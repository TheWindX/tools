using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    /*
     * 依次执行，有一项成功，退出成功，全失败退出失败，
     */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "合取(ANY)")]
    class COMScheduleSel : COMSchedule
    {
        public override void scheduleInit()
        {
            base.scheduleInit();
            mCurrent = scheduleGetChildren().GetEnumerator();
            mCurrent.MoveNext();
        }

        IEnumerator<COMSchedule> mCurrent = null;
        bool mExitValue = false;
        public override bool scheduleUpdate()
        {
            if (mCurrent == null) return true;
            var beh = mCurrent.Current;
            if (beh.getState() == ESTATE.e_uninit) beh.scheduleInit();
            var resUpdate = beh.scheduleUpdate();
            if (resUpdate)//当前子任务执行完成
            {
                mExitValue = beh.scheduleExit();
                if (mExitValue) return true;//有一执行成功
                if (!mCurrent.MoveNext())
                {
                    mExitValue = false;//全部执行失败
                    return true;
                }
            }
            return false;
        }

        private void reset()
        {
            mCurrent = null;
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
            if (mCurrent.Current != null)
            {
                mCurrent.Current.scheduleInterrupt();
            }
            reset();
        }
    }
}
