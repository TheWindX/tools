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
                return 
@"三个子任务，根据第一个任务是否成功，
切换第二个或第三个任务, 
永远执行，除非取消";
            }
        }

        public bool mRestart = false;
        public bool restart
        {
            get
            {
                return mRestart;
            }
            set
            {
                mRestart = value;
            }
        }

        public override bool scheduleBuild()
        {
            var children = scheduleGetChildren().ToList();
            if (children.Count() != 3)
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }
            mConditionSchedule = children[0];
            if(!mConditionSchedule.scheduleBuild())
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }
            mTrueSchedule = children[1];
            mFalseSchedule = children[2];
            if (mTrueSchedule == null)
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }   
            if (!mTrueSchedule.scheduleBuild())
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }
            if (mFalseSchedule == null)
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }   
            if (!mFalseSchedule.scheduleBuild())
            {
                MLogger.error("{0} is not build properly", getEditorObject().name);
                return false;
            }
            return true;
        }

        COMSchedule mConditionSchedule = null;
        COMSchedule mTrueSchedule = null;
        COMSchedule mFalseSchedule = null;
        public override void scheduleEnter()
        {
            base.scheduleEnter();
        }

        bool? mCondition = false;
        public override bool scheduleUpdate()
        {
            if (mConditionSchedule.getState() == ESTATE.e_exited)
                mConditionSchedule.scheduleEnter();
            var resUpdateCond = mConditionSchedule.scheduleUpdate();
            if (resUpdateCond)
            {
                var resExit = mConditionSchedule.scheduleExit();
                COMSchedule runSchedule = null;
                COMSchedule stopSchedule = null;
                if (mCondition == null) mCondition = !resExit;
                mCondition = resExit;

                if (resExit)
                {
                    runSchedule = mTrueSchedule;
                    stopSchedule = mFalseSchedule;
                }
                else
                {
                    runSchedule = mFalseSchedule;
                    stopSchedule = mTrueSchedule;
                }

                if(stopSchedule.getState() == ESTATE.e_entered)
                {
                    //MLogger.info("interrupt "+ stopSchedule.getEditorObject().name);
                    stopSchedule.scheduleInterrupt();
                }
                if(runSchedule.getState() == ESTATE.e_entered)
                {
                    if (mRestart)
                    {
                        //MLogger.info("restart " + runSchedule.getEditorObject().name);
                        runSchedule.scheduleInterrupt();
                        runSchedule.scheduleEnter();
                    }
                }
                else
                {
                    //MLogger.info("start " + runSchedule.getEditorObject().name);
                    runSchedule.scheduleEnter();
                }
            }

            if (mCondition != null)
            {
                if (mCondition.Value) mTrueSchedule.scheduleUpdate();
                else mFalseSchedule.scheduleUpdate();
                return false;
            }
            return false;
        }

        private void reset()
        {
            //mConditionSchedule = null;
            //mTrueSchedule = null;
            //mFalseSchedule = null;
            mCondition = null;
        }

        public override void scheduleInterrupt()
        {
            base.scheduleInterrupt();
            reset();
            if (mConditionSchedule.getState() == ESTATE.e_entered) mConditionSchedule.scheduleInterrupt();
            if (mTrueSchedule.getState() == ESTATE.e_entered) mTrueSchedule.scheduleInterrupt();
            if (mFalseSchedule.getState() == ESTATE.e_entered) mFalseSchedule.scheduleInterrupt();
        }
    }
}
