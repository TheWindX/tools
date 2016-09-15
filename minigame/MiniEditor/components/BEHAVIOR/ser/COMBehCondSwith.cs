//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MiniEditor
//{
//    /*
//     * 每次update，做condition判断, 发生变化时，切换子任务，子任务完成，退出
//     */
//    class COMBehCondSwitcher : COMBeh
//    {
//        protected COMBeh mTrueBeh = null;
//        protected COMBeh mFalseBeh = null;

//        bool mCondition = true;
//        public override void behInit()
//        {
//            var childrenIter = behGetChildren().GetEnumerator();
//            childrenIter.MoveNext();
//            mTrueBeh = childrenIter.Current;
//            childrenIter.MoveNext();
//            mFalseBeh = childrenIter.Current;
//            mCondition = checkCondition();
//        }

//        //子类中实现条件的判断
//        public virtual bool checkCondition()
//        {
//            return true;
//        }

//        public override bool behUpdate()
//        {
//            if (checkCondition())
//            {

//                if (mTrueBeh.getState() == ESTATE.e_uninit)
//                {
//                    mTrueBeh.behInit();
//                }
                

//            }
//            else
//            {
//                if (mFalseBeh.getState() == ESTATE.e_uninit)
//                    mFalseBeh.behInit();
//                return mFalseBeh.behUpdate();
//            }
//        }

//        private void reset()
//        {
//            mCondition = false;//此值init即为确定
//            mTrueBeh = null;
//            mFalseBeh = null;
//        }

//        public override bool behExit()
//        {
//            base.behExit();
//            bool ret = false;
//            if (mCondition)
//            {
//                ret = mTrueBeh.behExit();
//            }
//            else
//            {
//                ret = mTrueBeh.behExit();
//            }
//            reset();
//            return ret;
//        }

//        public override void behInterrupt()
//        {
//            base.behInterrupt();
//            if (mCondition)
//            {
//                mTrueBeh.behInterrupt();
//            }
//            else
//            {
//                mTrueBeh.behInterrupt();
//            }
//            reset();
//        }
//    }
//}
