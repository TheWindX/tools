using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
    condition: 三个子任务，根据第一个任务是否成功，执行返回第二个任务或第三个任务
    */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "条件(IF)")]
    class COMScheduleCond : COMSchedule
    {
        [Description]
        public string description
        {
            get
            {
                return "三个子任务，根据第一个任务是否成功，执行返回第二个任务或第三个任务";
            }
        }


        COMSchedule mConditionSchedule = null;
        COMSchedule mTrueSchedule = null;
        COMSchedule mFalseSchedule = null;
        public override void scheduleInit()
        {
            base.scheduleInit();
            //子类中实现条件的判断
            //mCondition = true;
            var childrenIter = scheduleGetChildren().GetEnumerator();
            childrenIter.MoveNext();
            mConditionSchedule = childrenIter.Current;
            childrenIter.MoveNext();
            mTrueSchedule = childrenIter.Current;
            childrenIter.MoveNext();
            mFalseSchedule = childrenIter.Current;
            mConditionSchedule.editorInit();
        }

        bool mConditionState = true;
        bool mCondition = true;
        bool mExitValue = false;
        public override bool scheduleUpdate()
        {
            if(mConditionState)
            {
                bool resUpdate = mConditionSchedule.scheduleUpdate();
                if(resUpdate)
                {
                    mCondition = mConditionSchedule.scheduleExit();
                    mConditionState = false;
                }
            }
            else
            {
                if(mCondition)
                {
                    if(mTrueSchedule.getState() == ESTATE.e_uninit)
                        mTrueSchedule.editorInit();
                    bool resUpdate = mTrueSchedule.scheduleUpdate();
                    if (resUpdate)
                    {
                        mExitValue = mTrueSchedule.scheduleExit();
                        return true;
                    }
                }
                else
                {
                    if (mFalseSchedule.getState() == ESTATE.e_uninit)
                        mFalseSchedule.editorInit();
                    bool resUpdate = mFalseSchedule.scheduleUpdate();
                    if (resUpdate)
                    {
                        mExitValue = mFalseSchedule.scheduleExit();
                        return true;
                    }
                }
            }

            return false;
        }

        private void reset()
        {
            mConditionSchedule = null;
            mTrueSchedule = null;
            mFalseSchedule = null;
            mConditionState = true;
            mCondition = true;
            mExitValue = false;
        }

        public override bool scheduleExit()
        {
            base.scheduleExit();
            var r = mExitValue;
            reset();
            return r;
        }

        public override void scheduleInterrupt()
        {
            base.scheduleInterrupt();
            

            reset();
        }
    }
}
