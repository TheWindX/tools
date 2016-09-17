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
        COMSchedule mChild = null;
        public override void scheduleInit()
        {
            base.scheduleInit();
            mChild = scheduleGetChildren().First();
            mChild.scheduleInit();
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
            mChild = null;
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
            mChild.scheduleInterrupt();
            reset();
        }
    }
}

