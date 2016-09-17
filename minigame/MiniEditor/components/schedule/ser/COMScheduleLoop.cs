﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
     * 循环(times)重试子任务，直到成功，次数结束，还未成功，返回失败
     */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "循环(LOOP)")]
    class COMScheduleLoop : COMSchedule
    {
        private int mTimes = -1;
        private int mTimeCount = 0;
        public int times
        {
            get
            {
                return mTimes;
            }
            set
            {
                mTimes = value;
            }
        }

        public bool always
        {
            get; set;
        }

        COMSchedule mChild = null;
        public override void scheduleInit()
        {
            base.scheduleInit();
            mChild = scheduleGetChildren().First();
            mChild.scheduleInit();
        }

        bool mExitValue = false;
        public override bool scheduleUpdate()
        {
            var resUpdate = mChild.scheduleUpdate();
            if(resUpdate)
            {
                mExitValue = mChild.scheduleExit(); 
                if(!always && mExitValue)
                {
                    return true;
                }
                else
                {
                    mTimeCount++;
                    if (mTimes >= 0 && mTimeCount >= mTimes)
                    {
                        return true;
                    }
                    mChild.scheduleInit();
                }
            }
            return false;
        }

        private void reset()
        {
            mChild = null;
            mTimeCount = 0;
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
            mChild.scheduleInterrupt();
            reset();
        }
    }
}
