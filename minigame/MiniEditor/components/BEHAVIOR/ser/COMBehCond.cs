using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
    三个子任务，根据第一个任务是否成功，执行返回第二个任务或第三个任务
    */
    class COMBehCond : COMBeh
    {
        COMBeh mConditionBeh = null;
        COMBeh mTrueBeh = null;
        COMBeh mFalseBeh = null;
        public override void behInit()
        {
            //子类中实现条件的判断
            //mCondition = true;
            var childrenIter = behGetChildren().GetEnumerator();
            childrenIter.MoveNext();
            mConditionBeh = childrenIter.Current;
            childrenIter.MoveNext();
            mTrueBeh = childrenIter.Current;
            childrenIter.MoveNext();
            mFalseBeh = childrenIter.Current;
        }

        bool mConditionState = true;
        bool mCondition = true;
        bool mExitValue = false;
        public override bool behUpdate()
        {
            if(mConditionState)
            {
                bool resUpdate = mConditionBeh.behUpdate();
                if(resUpdate)
                {
                    mCondition = mConditionBeh.behExit();
                    mConditionState = false;
                }
            }
            else
            {
                if(mCondition)
                {
                    bool resUpdate = mTrueBeh.behUpdate();
                    if (resUpdate)
                    {
                        mExitValue = mTrueBeh.behExit();
                        return true;
                    }
                }
                else
                {
                    bool resUpdate = mFalseBeh.behUpdate();
                    if (resUpdate)
                    {
                        mExitValue = mFalseBeh.behExit();
                        return true;
                    }
                }
            }

            return false;
        }

        private void reset()
        {
            mConditionBeh = null;
            mTrueBeh = null;
            mFalseBeh = null;
            mConditionState = true;
            mCondition = true;
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
            

            reset();
        }
    }
}
