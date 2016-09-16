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
    class COMBehReverse : COMBeh
    {
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
            if (resUpdate)
            {
                mExitValue = mChild.behExit();
            }
            return resUpdate;
        }

        private void reset()
        {
            mChild = null;
            mExitValue = false;
        }

        public override bool behExit()
        {
            base.behExit();
            var r = mExitValue;
            reset();
            return r;
        }

        public override void behInterrupt()
        {
            base.behInterrupt();
            mChild.behInterrupt();
            reset();
        }
    }
}

