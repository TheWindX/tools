using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_MiniGame
{
    class MRuntime
    {
        public static bool exit = false;

        internal static int mStart = 0;
        internal static int mNow = 0;

        internal static System.Action mEvtUpdate;
        internal static System.Action mEvtInit;
        internal static System.Action mEvtExit;
        internal static List<ModuleState> modules = new List<ModuleState>();
        internal static List<Action> mActions = new List<Action>();

        public static event System.Action evtFrame
        {
            add
            {
                mEvtUpdate += value;
            }
            remove
            {
                mEvtUpdate -= value;
            }
        }

        public static event System.Action evtInit
        {
            add
            {
                mEvtInit += value;
            }
            remove
            {
                mEvtInit -= value;
            }
        }


        public static event System.Action evtExit
        {
            add
            {
                mEvtExit += value;
            }
            remove
            {
                mEvtExit -= value;
            }
        }


        public static int getTime()
        {
            return mNow;
        }

        public static int getDeltaTime()
        {
            return mNow - mStart;
        }

        internal class ModuleState
        {
            public MModule mod;
            public enum EState
            {
                ePreInited,
                eInited,
                //eUpdate,
                eExited,
                eInvalid,
            }

            public EState state = EState.ePreInited;
        }


        public static void regModule(MModule mod)
        {
            mActions.Add(() =>
            {
                modules.Add(new ModuleState() { mod = mod, state = ModuleState.EState.ePreInited });
            });
        }

        public static void unregModule(MModule mod)
        {
            foreach (var m in modules)
            {
                if (m.mod == mod)
                {
                    if (m.state == ModuleState.EState.eInited)
                    {
                        m.state = ModuleState.EState.eExited;
                    }
                    return;
                }
            }
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class ModuleInstance : System.Attribute
    {
        private int level;

        public ModuleInstance(int level)
        {
            this.level = level;
        }
    }
}
