using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor.components.BEHAVIOR.par
{
    /* 
     * 同时执行，有一项成功，退出成功(同时打断所有进程)，全失败退出失败，
     */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "同时析取(ANY)")]
    class COMScheduleSelPar : COMSchedule
    {
        public override void scheduleInit()
        {
            base.scheduleInit();
            mChildren = scheduleGetChildren().ToList();
            foreach (COMSchedule beh in mChildren)
            {
                beh.scheduleInit();
            }
        }

        List<COMSchedule> mChildren = null;
        bool mExitValue = false;
        public override bool scheduleUpdate()
        {
            foreach (COMSchedule beh in mChildren.ToArray())
            {
                bool resUpdate = beh.scheduleUpdate();
                if (resUpdate)
                {
                    mExitValue = beh.scheduleExit();
                    mChildren.Remove(beh);
                    if (mExitValue)
                    {
                        foreach (COMSchedule beh1 in mChildren)
                        {
                            beh1.scheduleInterrupt();
                        }
                        return true;
                    }
                    else
                    {
                        if (mChildren.Count == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void reset()
        {
            mChildren = null;
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
            foreach (COMSchedule beh in mChildren)
            {
                beh.scheduleInterrupt();
            }
            reset();
        }
    }
}
