using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
    根据条件，开启分支任务，返回分支任务是否成功
    */
    class COMBehCond : COMBeh
    {
        protected bool mCondition = true;//此值init即为确定
        protected COMBeh mTrueBeh = null;
        protected COMBeh mFalseBeh = null;
        public override void behInit()
        {
            //子类中实现条件的判断
            //mCondition = true;
            
            var childrenIter = behGetChildren().GetEnumerator();
            childrenIter.MoveNext();
            mTrueBeh = childrenIter.Current;
            childrenIter.MoveNext();
            mFalseBeh = childrenIter.Current;
        }

        
        public override bool behUpdate()
        {
            if(mCondition)
            {
                if (mTrueBeh.getState() == ESTATE.e_uninit)
                    mTrueBeh.behInit();
                return mTrueBeh.behUpdate();
            }
            else
            {
                if (mFalseBeh.getState() == ESTATE.e_uninit)
                    mFalseBeh.behInit();
                return mFalseBeh.behUpdate();
            }
        }

        private void reset()
        {
            mCondition = false;//此值init即为确定
            mTrueBeh = null;
            mFalseBeh = null;
        }

        public override bool behExit()
        {
            base.behExit();
            bool ret = false;
            if (mCondition)
            {
                ret = mTrueBeh.behExit();
            }
            else
            {
                ret = mTrueBeh.behExit();
            }
            reset();
            return ret;
        }

        public override void behInterrupt()
        {
            base.behInterrupt();
            if (mCondition)
            {
                mTrueBeh.behInterrupt();
            }
            else
            {
                mTrueBeh.behInterrupt();
            }
            reset();
        }
    }
}
