using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_vision
{
    public partial class CRuntime
    {
        public CModuleTree currentSpace = null;
        public CModuleItem selected = null;

        public CRuntime()
        {
            reset();
        }

        public void reset()
        {
            currentSpace = createModule(null, "root").getComponent<CModuleTree>();
            selected = null;

            rootModule = currentSpace.getComponent<CModule>();
        }

        #region test1
        void mkFold(string name)
        {
            currentSpace = createFold(currentSpace, name).getComponent<CModuleTree>(); ;
            selected = null;
        }

        void mkModule(string name)
        {
            currentSpace = createModule(currentSpace, name).getComponent<CModuleTree>(); ;
            selected = null;
        }

        void cd(string name)
        {
            var s = currentSpace.children.First(item =>
            {
                return item.getComponent<CNamed>().name == name;
            });
            var t = s.getComponent<CModuleTree>();
            if(t != null)
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

        void mkInt(int v)
        {
            selected = createIntValue(currentSpace, v).getComponent<CModuleItem>();
        }

        public void test1()
        {
            mkFold("fa");
            mkFold("faa");
            mkFold("faaa");
            cdback();
            cdback();
            mkModule("ma");
            mkModule("maa");
            mkModule("maaa");
            cdback();
            mkInt(100);
            var mt = rootModule.getComponent<CModuleItem>();
            mt.print(0);
        }
        #endregion test1

        #region test2
        public void test2()
        {
            mkFold("fa");
            mkFold("faa");
            mkFold("faaa");
            cdback();
            cdback();
            mkModule("ma");
            mkModule("maa");
            mkModule("maaa");
            cdback();
            mkInt(100);

            var mt = rootModule.getComponent<CModuleTree>();
            RuntimeUtil.Instance.SetCurrentSpace(mt);
        }
        #endregion test2
    }
}
