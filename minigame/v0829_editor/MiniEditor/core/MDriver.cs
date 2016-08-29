using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    class MDriver
    {
        static Stopwatch mStopWatch = new Stopwatch();

        
        static void addAssemblyModules()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var ts = myAssembly.GetTypes();
            
            List<Type> mAttrModules = new List<Type>();
            foreach (var t in ts)
            {
                var attrs = t.GetCustomAttribute<ModuleInstance>();
                if (attrs != null)
                {
                    //MModule instance = (MModule)Activator.CreateInstance(t);
                    mAttrModules.Add(t);
                }
            }
            mAttrModules.Sort((t1, t2) =>
            {
                var att1 = t1.GetCustomAttribute<ModuleInstance>();
                var att2 = t2.GetCustomAttribute<ModuleInstance>();
                if (att1.level < att2.level) return -1;
                else if (att1.level < att2.level) return 0;
                else return 1;
            });

            foreach (var t in mAttrModules)
            {
                MModule instance = (MModule)Activator.CreateInstance(t);
                MRuntime.regModule(instance);
            }
        }

        internal static void init()
        {
            addAssemblyModules();

            mStopWatch.Start();
            MRuntime.mStart = (int)mStopWatch.ElapsedMilliseconds;
            MRuntime.mLast = MRuntime.mStart;
            MTimer.Init(() => (uint)mStopWatch.ElapsedMilliseconds);

            try
            {
                if (MRuntime.mEvtInit != null) MRuntime.mEvtInit();
            }
            catch (Exception ex)
            {
                MLogger.error(ex.Message);
            }
        }

        static List<MRuntime.ModuleState> toRemoved = new List<MRuntime.ModuleState>();
        internal static void update()
        {
            toRemoved.Clear();
            MRuntime.mNow = (int)mStopWatch.ElapsedMilliseconds;
            MTimer.tickAll();
            try
            {
                if (MRuntime.mEvtUpdate != null) MRuntime.mEvtUpdate();
            }
            catch (Exception ex)
            {
                MLogger.error(ex.Message);
            }

            while(MRuntime.mActions.Count != 0)
            {
                int idx = MRuntime.mActions.Count - 1;
                try
                {
                    MRuntime.mActions[idx]();
                }
                catch (Exception ex)
                {
                    MLogger.error(ex.Message);
                }
                MRuntime.mActions.RemoveAt(idx);
            }

            foreach (var mod in MRuntime.modules)
            {
                if (mod.state == MRuntime.ModuleState.EState.eInvalid)
                {
                    toRemoved.Add(mod);
                }

                if (mod.state == MRuntime.ModuleState.EState.eExited)
                {
                    toRemoved.Add(mod);
                    try
                    {
                        mod.mod.onExit();
                    }
                    catch (Exception ex)
                    {
                        MLogger.error(ex.Message);
                        mod.state = MRuntime.ModuleState.EState.eInvalid;
                    }
                }
                else if (mod.state == MRuntime.ModuleState.EState.ePreInited)
                {
                    try
                    {
                        mod.mod.onInit();
                        mod.state = MRuntime.ModuleState.EState.eInited;
                    }
                    catch (Exception ex)
                    {
                        MLogger.error(ex.Message);
                        mod.state = MRuntime.ModuleState.EState.eInvalid;
                    }
                }
                else if (mod.state == MRuntime.ModuleState.EState.eInited)
                {
                    try
                    {
                        mod.mod.onUpdate();
                    }
                    catch (Exception ex)
                    {
                        MLogger.error(ex.Message);
                        //mod.state = MRuntime.ModuleState.EState.eInvalid;
                    }
                }
            }

            foreach (var rmv in toRemoved)
            {
                MRuntime.modules.Remove(rmv);
            }
            MRuntime.mLast = MRuntime.mNow;
        }

        internal static void exit()
        {
            mStopWatch.Stop();
            try
            {
                if (MRuntime.mEvtExit != null) MRuntime.mEvtExit();
            }
            catch (Exception ex)
            {
                MLogger.error(ex.Message);
            }

            foreach (var mod in MRuntime.modules)
            {
                if (mod.state == MRuntime.ModuleState.EState.eExited)
                {
                    try
                    {
                        mod.mod.onExit();
                    }
                    catch (Exception ex)
                    {
                        MLogger.error(ex.Message);
                        mod.state = MRuntime.ModuleState.EState.eInvalid;
                    }
                }
                else if (mod.state == MRuntime.ModuleState.EState.eInited)
                {
                    try
                    {
                        mod.mod.onExit();
                    }
                    catch (Exception ex)
                    {
                        MLogger.error(ex.Message);
                        mod.state = MRuntime.ModuleState.EState.eInvalid;
                    }
                }
            }
            MRuntime.modules.Clear();
        }
    }
}
