using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
     * 依次执行，有一项失败，退出失败，全成功退出成功，
     */
    [CustomComponent(path = "BEHAVIOR", name = "序列")]
    class COMBehSeq : COMBeh
    {
        public override void behInit()
        {
            base.behInit();
            mCurrent = behGetChildren().GetEnumerator();
            mCurrent.MoveNext();
        }


        IEnumerator<COMBeh> mCurrent = null;
        bool mExitValue = false;
        public override bool behUpdate()
        {
            if (mCurrent == null) return true;
            var beh = mCurrent.Current;
            if (beh.getState() == ESTATE.e_uninit) beh.behInit();
            var resUpdate = beh.behUpdate();
            if(resUpdate)//当前子任务执行完成
            {
                mExitValue = beh.behExit();
                if (!mExitValue) return true;//有一执行失败
                if (!mCurrent.MoveNext())
                {
                    mExitValue = true;//全部执行成功
                    return true;
                }
            }
            return false;
        }

        private void reset()
        {
            mCurrent = null;
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
            if (mCurrent.Current != null)
            {
                mCurrent.Current.behInterrupt();
            }
            reset();
        }
    }
}
