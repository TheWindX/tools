using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ns_YAUtils;

namespace ns_YAMini
{
    class TimerProvide : Singleton<TimerProvide>
    {
        uint mTime;
        public void updateTimer()
        {
            var tmp = (uint)(System.DateTime.Now.Ticks / 10000 << 4 >> 4);
            if (tmp > mTime)
                mTime = tmp;
        }

        public uint nowTimer()
        {
            return mTime;
        }
    };

    class TLoop : Singleton<TLoop>
    {
        public void update()
        {
            if (evtUpdate != null)
                evtUpdate();
        }

        public bool exit = false;

        public Action evtUpdate;
    };

    class Program
    {

        static void Main(string[] args)
        {
            initAll();
            while (TLoop.Instance.exit == false)
            {
                TLoop.Instance.update();
            }
        }

        static void initAll()
        {
            init();
            test1();
        }

        static void init()
        {
            var repl = ns_YAUtils.CSRepl.Instance;
            repl.start();

            TimerProvide.Instance.updateTimer();
            ns_YAUtils.TimerManager.Init(TimerProvide.Instance.nowTimer);
            TLoop.Instance.evtUpdate += TimerProvide.Instance.updateTimer;
            TLoop.Instance.evtUpdate += ns_YAUtils.TimerManager.tickAll;
            TLoop.Instance.evtUpdate += repl.runOnce;
        }

        static bool toggle = false;


        static int clickEvent = -1;

        static uint timeInterval = 500;


        static void upInterval()
        {
            timeInterval = (uint)(timeInterval / 1.1);
            stopAutoClick();
            autoClick();
        }

        static void downInterval()
        {
            timeInterval = (uint)(timeInterval * 1.1); 
            stopAutoClick();
            autoClick();
        }

        static void autoClick()
        {
            if (clickEvent != -1)
                ns_YAUtils.TimerManager.get().clearTimer(clickEvent);
            clickEvent = ns_YAUtils.TimerManager.get().setInterval((i, t) =>
            {
                InputEvents.click();
            }, timeInterval, null,
            4211111111);
            toggle = false;
        }
        static void stopAutoClick()
        {
            if (clickEvent != -1)
                ns_YAUtils.TimerManager.get().clearTimer(clickEvent);
            toggle = true;
        }



        static void test1()
        {

            ns_YAUtils.TimerManager.get().setInterval((i, t) =>
            {
                if (InputEvents.GetKeyPressState((int)VirtualKeyStates.VK_LCONTROL))
                {
                    autoClick();
                }
                else if (InputEvents.GetKeyPressState((int)VirtualKeyStates.VK_RETURN))
                {
                    stopAutoClick();
                }
                else if (InputEvents.GetKeyPressState((int)VirtualKeyStates.VK_UP))
                {
                    upInterval();
                }
                else if (InputEvents.GetKeyPressState((int)VirtualKeyStates.VK_DOWN))
                {
                    downInterval();
                }
            }, 100,null, 4211111111);
        }
    }
}

