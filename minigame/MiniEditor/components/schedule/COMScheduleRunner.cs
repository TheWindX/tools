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
     *  触发同一个物体上的COMSchedule
     */
    [CustomComponent(path = "SCHEDULE/UTIL", name = "Schedule测试")]
    class COMScheduleRunner : MComponent
    {
        COMSchedule mSchedule = null;
        public bool update()
        {
            return mSchedule.scheduleUpdate();
        }

        bool mExitValue = false;
        public bool result
        {
            get
            {
                return mExitValue;
            }
            set
            {
                mExitValue = value;
            }
        }

        //add component或enable时回调
        public override void editorAwake()
        {
            
        }

        //remove component或disable时回调
        public override void editorSleep()
        {
            run = false;
        }

        //进入editor object时回调
        public override void editorInit()
        {
            //MLogger.info("editorInit: {0}", GetType().Name);
        }
        
        //离开editor object时回调
        public override void editorExit()
        {
            run = false;
        }

        public override void editorUpdate()
        {
            if(run)
            {   
                if(update())
                {
                    mExitValue = mSchedule.scheduleExit();
                    run = false;
                }
            }
        }

        private bool mRun = false;
        public bool run
        {
            get
            {
                return mRun;
            }
            set
            {
                mSchedule = getComponent<COMSchedule>();
                if (mRun == value) return;
                mRun = value;
                if(mRun)
                {   
                    if(mSchedule != null)
                    {
                        mSchedule.scheduleEnter();
                    }
                }
                else
                {
                    if (mSchedule != null)
                    {
                        if(mSchedule.getState() == COMSchedule.ESTATE.e_entered)
                        {
                            mSchedule.scheduleInterrupt();
                        }
                    }
                }
            }
        }//run

    }
}
