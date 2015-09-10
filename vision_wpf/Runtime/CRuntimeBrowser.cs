using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_vision
{
    public partial class CRuntimeBrowser : Component
    {
        public CModuleTree currentSpace = null;
        public CModuleItem selected = null;

        public void resetTop()
        {
            currentSpace = currentSpace.getComponent<CRuntimeObj>().runtime.rootModule.getComponent<CModuleTree>();
            selected = null;
        }
        
        public void mkFold(string name)
        {
            currentSpace = currentSpace.createFold(name).getComponent<CModuleTree>(); ;
            selected = null;
        }

        public void mkModule(string name)
        {
            currentSpace = currentSpace.createModule(name).getComponent<CModuleTree>(); ;
            selected = null;
        }

        public void cd(string name)
        {
            var s = currentSpace.children.First(item =>
            {
                return item.getComponent<CNamed>().name == name;
            });
            var t = s.getComponent<CModuleTree>();
            if (t != null)
            {
                currentSpace = t;
                selected = null;
                return;
            }
            RuntimeUtil.Instance.log("no found");
        }

        public void cdback()
        {
            var mi = currentSpace.getComponent<CModuleItem>();
            if (mi.parent != null)
            {
                selected = mi;
                currentSpace = mi.parent;
            }
        }

        public void mkInt(int v)
        {
            selected = currentSpace.createIntValue(v).getComponent<CModuleItem>();
        }
    }
}
