/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
     * 依次执行，有一项成功，退出成功，全失败退出失败，
     */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "析取(ANY)")]
    class COMScheduleSel : COMSchedule
    {
        public override bool scheduleBuild()
        {
            var children = scheduleGetChildren().ToList();
            if (children.Count() == 0)
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }
            bool res = false;
            foreach (var c in children)
            {
                res = c.scheduleBuild();
                if (!res)
                {
                    MLogger.error("{0} is not build properly", getEditorObject().name);
                    return false;
                }
            }
            mCurrent = scheduleGetChildren().ToList();
            idx = 0;
            return true;
        }

        public override void scheduleEnter()
        {
            base.scheduleEnter();
        }

        List<COMSchedule> mCurrent = null;
        int idx = 0;
        bool mExitValue = false;
        public override bool scheduleUpdate()
        {
            if (mCurrent == null) return true;
            var beh = mCurrent[idx];
            if (beh.getState() == ESTATE.e_exited) beh.scheduleEnter();
            var resUpdate = beh.scheduleUpdate();
            if (resUpdate)//当前子任务执行完成
            {
                mExitValue = beh.scheduleExit();
                if (mExitValue) return true;//有一执行失败
                idx++;
                if (idx == mCurrent.Count)
                {
                    mExitValue = false;//全部执行成功
                    return true;
                }
            }
            return false;
        }

        private void reset()
        {
            idx = 0;
            mExitValue = false;
        }

        public override bool scheduleExit()
        {
            base.scheduleExit();
            var r = mExitValue;
            reset();
            return r;
        }

        public override void scheduleInterrupt()
        {
            base.scheduleInterrupt();
            var beh = mCurrent[idx];
            beh.scheduleInterrupt();
        }
    }
}
