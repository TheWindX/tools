﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
    condition: 三个子任务，根据第一个任务是否成功，执行返回第二个任务或第三个任务
    */
    [CustomComponent(path = "schedule", name = "条件")]
    class COMScheduleCond : COMSchedule
    {
        COMSchedule mConditionSchedule = null;
        COMSchedule mTrueSchedule = null;
        COMSchedule mFalseSchedule = null;
        public override void scheduleInit()
        {
            //子类中实现条件的判断
            //mCondition = true;
            var childrenIter = scheduleGetChildren().GetEnumerator();
            childrenIter.MoveNext();
            mConditionSchedule = childrenIter.Current;
            childrenIter.MoveNext();
            mTrueSchedule = childrenIter.Current;
            childrenIter.MoveNext();
            mFalseSchedule = childrenIter.Current;
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
                    bool resUpdate = mTrueSchedule.scheduleUpdate();
                    if (resUpdate)
                    {
                        mExitValue = mTrueSchedule.scheduleExit();
                        return true;
                    }
                }
                else
                {
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
