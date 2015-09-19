using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ns_vision
{
    public partial class CRuntime : Component
    {
        public CModule rootModule { get; private set; }
        public override void init()
        {
            reset();
        }


        public void reset()
        {
            var com = Component.create<CModule>(new ComponentContainer());
            com.getComponent<CNamed>()._HandleNamed = () => "root";
            com.getComponent<CRuntimeObj>().runtime = this;
            com.getComponent<CModuleItem>().parent = null;
            rootModule = com;
        }

        public CBrowserModuleTree createBrowser(CModuleTree mt)
        {
            var bs = new CBrowserModuleTree();
            bs.currentSpace = mt;
            bs.selected = null;
            return bs;
        }
    }

}