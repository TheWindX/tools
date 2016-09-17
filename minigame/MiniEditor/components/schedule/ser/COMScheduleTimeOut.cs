using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
     * 过一段时间自动成功退出
     */
    [CustomComponent(path = "BEHAVIOR", name = "持续时间")]
    class COMScheduleTimeOut : COMSchedule
    {
        private int mTimeOut = 0;
        public int time
        {
            get
            {
                return mTimeOut;
            }
            set
            {
                mTimeOut = value;
            }
        }

        private int mTimeCount = 0;
        public override void scheduleInit()
        {
            base.scheduleInit();
            mTimeCount = 0;
        }

        public override bool scheduleUpdate()
        {
            mTimeCount += MRuntime.getDeltaTime();
            if(mTimeCount > mTimeOut)
            {
                return true;
            }
            return false;
        }

        private void reset()
        {
            mTimeCount = 0;
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
            reset();
        }
    }
}
