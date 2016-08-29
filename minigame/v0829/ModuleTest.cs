using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_MiniGame
{
    [ns_MiniGame.ModuleInstance(0)]
    class ModuleTest : MModule
    {
        public void onExit()
        {
            Console.WriteLine("onExit");
        }

        public void onInit()
        {
            Console.WriteLine("onInit");
            MTimer.get().setTimeout(t =>
            {
                MRuntime.unregModule(this);
            }, 500);

            MTimer.get().setTimeout(t =>
            {
                MRuntime.exit = true;
            }, 1500);
        }

        public void onUpdate()
        {
            Console.WriteLine("onUpdate");
        }
    }
}
