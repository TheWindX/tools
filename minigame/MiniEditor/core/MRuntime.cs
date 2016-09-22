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
    class MRuntime
    {
        public static bool exit = false;

        internal static int mStart = 0;
        internal static int mLast = 0;
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
            return mNow - mStart;
        }

        public static int getDeltaTime()
        {
            return mNow - mLast;
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

    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = true)]
    public class ModuleInstance : System.Attribute
    {
        public int level;

        public ModuleInstance(int level)
        {
            this.level = level;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = true)]
    public class CustomComponentAttribute : System.Attribute
    {
        public string mPath = "";
        public string path
        {
            get
            {
                return mPath;
            }
            set
            {
                mPath = value;
            }
        }

        public string mName = "";
        public string name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private bool mRemovable = true;
        public bool removable
        {
            get
            {
                return mRemovable;
            }
            set
            {
                mRemovable = value;
            }
        }

        private bool mEditable = true;
        public bool editable
        {
            get
            {
                return mEditable;
            }
            set
            {
                mEditable = value;
            }
        }

        //private bool mIsMain = false;
        //public bool isMain
        //{
        //    get
        //    {
        //        return mIsMain;
        //    }
        //    set
        //    {
        //        mIsMain = value;
        //    }
        //}
    }
    
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PropertyDisableAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DescriptionAttribute : System.Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CustomInspectorAttribute : System.Attribute
    {
    }
}
