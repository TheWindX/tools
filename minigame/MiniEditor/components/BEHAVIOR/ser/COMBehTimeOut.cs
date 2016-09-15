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
    class COMBehTimeOut : COMBeh
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
        public override void behInit()
        {
            base.behInit();
            mTimeCount = 0;
        }

        public override bool behUpdate()
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

        public override bool behExit()
        {
            base.behExit();
            reset();
            return true;
        }

        public override void behInterrupt()
        {
            base.behInterrupt();
            reset();
        }
    }
}
