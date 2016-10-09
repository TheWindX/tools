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
     * 对子任务的运行结果取反
     */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "取反(!)")]
    class COMScheduleReverse : COMSchedule
    {
        public override bool scheduleBuild()
        {
            var children = scheduleGetChildren().ToList();
            if (children.Count() != 1)
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }
            mChild = children[0];
            var res = mChild.scheduleBuild();
            if (!res)
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }
            return true;
        }

        COMSchedule mChild = null;
        public override void scheduleEnter()
        {
            base.scheduleEnter();
            mChild.scheduleEnter();
        }

        bool mExitValue = false;
        public override bool scheduleUpdate()
        {
            var resUpdate = mChild.scheduleUpdate();
            if (resUpdate)
            {
                mExitValue = mChild.scheduleExit();
            }
            return resUpdate;
        }

        private void reset()
        {
            mExitValue = false;
        }

        public override bool scheduleExit()
        {
            base.scheduleExit();
            var r = !mExitValue;
            reset();
            return r;
        }

        public override void scheduleInterrupt()
        {
            base.scheduleInterrupt();
            mChild.scheduleInterrupt();
            reset();
        }
    }
}

