using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ns_MiniGame
{
    class MDriver
    {
        static Stopwatch mStopWatch = new Stopwatch();

        static void addAssemblyModules()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var ts = myAssembly.GetTypes();
            foreach (var t in ts)
            {
                var attrs = t.GetCustomAttribute<ModuleInstance>();
                if (attrs != null)
                {
                    MModule instance = (MModule)Activator.CreateInstance(t);
                    MRuntime.regModule(instance);
                }
            }
        }

        internal static void init()
        {
            addAssemblyModules();

            mStopWatch.Start();
            MRuntime.mStart = (int)mStopWatch.ElapsedMilliseconds;
            MTimer.Init(() => (uint)mStopWatch.ElapsedMilliseconds);

            try
            {
                if (MRuntime.mEvtInit != null) MRuntime.mEvtInit();
            }
            catch (Exception ex)
            {

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
                    try
                    {
                        mod.mod.onExit();
                    }
                    catch (Exception ex)
                    {
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
                        //mod.state = MRuntime.ModuleState.EState.eInvalid;
                    }
                }
            }

            foreach (var rmv in toRemoved)
            {
                MRuntime.modules.Remove(rmv);
            }
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
                        mod.state = MRuntime.ModuleState.EState.eInvalid;
                    }
                }
            }
            MRuntime.modules.Clear();
        }
    }
}
