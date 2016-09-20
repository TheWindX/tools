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
     * 依次执行，有一项失败，退出失败，全成功退出成功，
     */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "合取(ALL)")]
    class COMScheduleSeq : COMSchedule
    {
        public override bool scheduleInit()
        {
            var children = scheduleGetChildren().ToList();
            if (children.Count() == 0)
            {
                return false;
            }
            bool res = false;
            foreach (var c in children)
            {
                res = c.scheduleInit();
                if (!res)
                {
                    return false;
                }
            }
            mCurrent = scheduleGetChildren().GetEnumerator();
            mCurrent.MoveNext();
            return true;
        }

        public override void scheduleEnter()
        {
            base.scheduleEnter();
        }


        IEnumerator<COMSchedule> mCurrent = null;
        bool mExitValue = false;
        public override bool scheduleUpdate()
        {
            if (mCurrent == null) return true;
            var beh = mCurrent.Current;
            if (beh.getState() == ESTATE.e_exited) beh.scheduleEnter();
            var resUpdate = beh.scheduleUpdate();
            if(resUpdate)//当前子任务执行完成
            {
                mExitValue = beh.scheduleExit();
                if (!mExitValue) return true;//有一执行失败
                if (!mCurrent.MoveNext())
                {
                    mExitValue = true;//全部执行成功
                    return true;
                }
            }
            return false;
        }

        private void reset()
        {
            mCurrent = null;
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
            if (mCurrent.Current != null)
            {
                mCurrent.Current.scheduleInterrupt();
            }
            reset();
        }
    }
}
