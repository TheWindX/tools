using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor.components.BEHAVIOR.par
{
    /*
     * 同时执行，有一项失败，退出失败(同时打断所有进程)，全成功退出成功，
     */
    class COMBehSeqPar : COMBeh
    {
        public override void behInit()
        {
            base.behInit();
            mChildren = behGetChildren().ToList();
            foreach (COMBeh beh in mChildren)
            {
                beh.behInit();
            }
        }

        List<COMBeh> mChildren = null;
        bool mExitValue = false;
        public override bool behUpdate()
        {
            foreach(COMBeh beh in mChildren.ToArray())
            {
                bool resUpdate = beh.behUpdate();
                if(resUpdate)
                {
                    mExitValue = beh.behExit();
                    mChildren.Remove(beh);
                    if (!mExitValue)
                    {
                        foreach(COMBeh beh1 in mChildren)
                        {
                            beh1.behInterrupt();
                        }
                        return true;
                    }
                    else
                    {
                        if (mChildren.Count == 0)
                        {
                            return true;
                        }   
                    }
                }
                return false;
            }
            return true;
        }

        private void reset()
        {
            mChildren = null;
            mExitValue = false;
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
            foreach(COMBeh beh in mChildren)
            {
                beh.behInterrupt();
            }
            reset();
        }
    }
}
