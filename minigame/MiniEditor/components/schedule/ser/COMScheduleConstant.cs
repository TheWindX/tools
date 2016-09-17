using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
    执行子任务，但总是返回成功或失败
    */
    [CustomComponent(path = "schedule", name = "返回常量")]
    class COMScheduleConstant : COMSchedule
    {
        private bool mConstant = true;
        public bool exitValue
        {
            get
            {
                return mConstant;
            }
            set
            {
                mConstant = value;
            }
        }

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
            return mConstant;
        }

        public override void scheduleInterrupt()
        {
            base.scheduleInterrupt();
            mChild.scheduleInterrupt();
            reset();
        }
    }
}
