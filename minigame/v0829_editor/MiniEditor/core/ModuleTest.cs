using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    
    class MComponent1 : MComponent
    {
        public int v = 1000;
    }

    [RequireCom(typeof(MComponent1))]
    class MComponent2 : MComponent
    {
        public string v = "asdf";
    }

    [ModuleInstance(0)]
    class ModuleTest : MModule
    {
        public void onExit()
        {
            MLogger.info("exit");
        }

        public void onInit()
        {
            EditObject obj = new EditObject();
            var c = obj.getOrAddComponent<MComponent2>();
            
            MLogger.info("onInit");
            MTimer.get().setTimeout(t =>
            {
                MRuntime.unregModule(this);
            }, 500);

            MTimer.get().setTimeout(t =>
            {
                MRuntime.exit = true;
                MLogger.info(c.getComponent<MComponent1>().v.ToString());
                MLogger.info(c.getComponent<MComponent2>().v.ToString());
                c.removeComponent<MComponent2>();
                //MLogger.info(c.getComponent<MComponent1>().v);
            }, 1500);
        }

        public void onUpdate()
        {
            MLogger.info("onUpdate");
        }
    }
}
