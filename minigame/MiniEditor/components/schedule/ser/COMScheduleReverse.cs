using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
     * 循环重试子任务，直到成功
     */
    [CustomComponent(path = "BEHAVIOR", name = "取反")]
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

