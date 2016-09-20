/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
    condition: 三个子任务，根据第一个任务是否成功，切换第二个任务或第三个任务
    */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "条件切换(switch)")]
    class COMScheduleSwitch : COMSchedule
    {
        [Description]
        public string description
        {
            get
            {
                return "三个子任务，根据第一个任务是否成功，切换第二个任务或第三个任务";
            }
        }

        public override bool scheduleInit()
        {
            var children = scheduleGetChildren().ToList();
            if (children.Count() != 3)
            {
                return false;
            }
            mConditionSchedule = children[0];
            if(!mConditionSchedule.scheduleInit())
            {
                return false;
            }
            mTrueSchedule = children[1] as COMScheduleThread;
            mFalseSchedule = children[2] as COMScheduleThread;
            if (mTrueSchedule == null)
                return false;
            if (!mTrueSchedule.scheduleInit())
            {
                return false;
            }
            if (mFalseSchedule == null)
                return false;
            if (!mFalseSchedule.scheduleInit())
            {
                return false;
            }
            return true;
        }

        COMSchedule mConditionSchedule = null;
        COMScheduleThread mTrueSchedule = null;
        COMScheduleThread mFalseSchedule = null;
        public override void scheduleEnter()
        {
            base.scheduleEnter();
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
                    if (mFalseSchedule.getState() == ESTATE.e_entered)
                        mFalseSchedule.scheduleInterrupt();
                    if (mTrueSchedule.getState() == ESTATE.e_exited)
                        mTrueSchedule.scheduleEnter();
                    bool resUpdate = mTrueSchedule.scheduleUpdate();
                    if (resUpdate)
                    {
                        mExitValue = mTrueSchedule.scheduleExit();
                        return true;
                    }
                }
                else
                {
                    if (mTrueSchedule.getState() == ESTATE.e_entered)
                        mTrueSchedule.scheduleInterrupt();
                    if (mFalseSchedule.getState() == ESTATE.e_exited)
                        mFalseSchedule.scheduleEnter();
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
