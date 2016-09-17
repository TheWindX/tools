using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
    执行子任务，总是返回成功
    */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "永真(TRUE)")]
    class COMScheduleTrue : COMSchedule
    {
        COMSchedule mChild = null;
        public override void scheduleInit()
        {
            base.scheduleInit();
            mChild = scheduleGetChildren().First();
            mChild.scheduleInit();
        }
        
        public override bool scheduleUpdate()
        {
            var resUpdate = mChild.scheduleUpdate();
            return resUpdate;
        }

        private void reset()
        {
            mChild = null;
        }

        public override bool scheduleExit()
        {
            base.scheduleExit();
            reset();
            return true;
        }

        public override void scheduleInterrupt()
        {
            base.scheduleInterrupt();
            mChild.scheduleInterrupt();
            reset();
        }
    }
}
