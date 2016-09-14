using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [ModuleInstance(0)]
    class ModuleConsole : MModule
    {
        public void onExit()
        {
            CSRepl.stop();
        }

        public void onInit()
        {
            CSRepl.Instance.start();
        }

        public void onUpdate()
        {
            CSRepl.Instance.runOnce();
        }
    }
}
