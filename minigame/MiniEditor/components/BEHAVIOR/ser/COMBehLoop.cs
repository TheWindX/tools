using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
     * 循环(times)重试子任务，直到成功，次数结束，还未成功，返回失败
     */
    [CustomComponent(path = "BEHAVIOR", name = "循环")]
    class COMBehLoop : COMBeh
    {
        private int mTimes = 1;
        private int mTimeCount = 0;
        public int times
        {
            get
            {
                return mTimes;
            }
            set
            {
                mTimes = value;
            }
        }

        COMBeh mChild = null;
        public override void behInit()
        {
            base.behInit();
            mChild = behGetChildren().First();
            mChild.behInit();
        }

        bool mExitValue = false;
        public override bool behUpdate()
        {
            var resUpdate = mChild.behUpdate();
            if(resUpdate)
            {
                mExitValue = mChild.behExit(); 
                if(mExitValue)
                {
                    return true;
                }
                else
                {
                    mTimeCount++;
                    if (mTimeCount >= mTimes)
                    {
                        return true;
                    }
                    mChild.behInit();
                }
            }
            return false;
        }

        private void reset()
        {
            mChild = null;
            mTimeCount = 0;
        }

        public override bool behExit()
        {
            base.behExit();
            reset();
            return mExitValue;
        }

        public override void behInterrupt()
        {
            base.behInterrupt();
            mChild.behInterrupt();
            reset();
        }
    }
}
