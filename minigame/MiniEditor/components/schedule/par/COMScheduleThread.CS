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
    开启一个线程，不阻塞立刻返回真，下次调用时，根据线程是否结束，再次重启
    //TODO：NOTE, 目前有泄露风险，需手工保证thread 处于 时移除
    */
    [CustomComponent(path = "SCHEDULE/COMBINE", name = "线程(\"THREAD\")")]
    class COMScheduleThread : COMSchedule
    {
        bool running
        {
            get
            {
                if(mChild == null)
                {
                    return false;
                }
                else
                {
                    return mChild.getState() == ESTATE.e_inited;
                }
            }
            set
            {
                if (value)
                    scheduleUpdate();
                else
                    scheduleInterrupt();
            }
        }

        COMSchedule mChild = null;
        public override void scheduleInit()
        {
            base.editorInit();
            var childrenIter = scheduleGetChildren().GetEnumerator();
            childrenIter.MoveNext();
            mChild = childrenIter.Current;
        }

        public override bool scheduleUpdate()
        {
            if (mChild.getState() == ESTATE.e_inited) return true;
            mChild.editorInit();
            MRuntime.mEvtUpdate -= childUpdate;
            MRuntime.mEvtUpdate += childUpdate;
            return true;
        }

        public void childUpdate()
        {
            bool resUpdate = mChild.scheduleUpdate();
            if(resUpdate)
            {
                MRuntime.mEvtUpdate -= childUpdate;
                mChild.editorExit();
            }
        }

        private void reset()
        {
           
        }

        public override bool scheduleExit()
        {
            base.scheduleExit();
            reset();
            return true;
        }

        public override void scheduleInterrupt()
        {
            base.scheduleInterrupt();
            if(mChild.getState() == ESTATE.e_inited)
            {
                mChild.scheduleInterrupt();
                MRuntime.mEvtUpdate -= childUpdate;
            }
            reset();
        }
    }
}
