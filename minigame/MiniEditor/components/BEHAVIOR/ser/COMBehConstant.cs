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
    [CustomComponent(path = "BEHAVIOR", name = "常量")]
    class COMBehConstant : COMBeh
    {
        private bool mExitValue = true;
        public bool exitValue
        {
            get
            {
                return mExitValue;
            }
            set
            {
                mExitValue = value;
            }
        }

        COMBeh mChild = null;
        public override void behInit()
        {
            base.behInit();
            mChild = behGetChildren().First();
            mChild.behInit();
        }
        
        public override bool behUpdate()
        {
            var resUpdate = mChild.behUpdate();
            return resUpdate;
        }

        private void reset()
        {
            mChild = null;
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
