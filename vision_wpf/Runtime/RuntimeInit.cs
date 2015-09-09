using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_vision
{
    public partial class CRuntime
    {
        public CModule createModule(CModuleTree parent, string name)
        {
            var com = Component.create<CModule>(new ComponentContainer());
            com.getComponent<CNamed>()._nameHandle = () => name;
            com.getComponent<CRuntimeObj>().runtime = this;
            com.getComponent<CModuleItem>().parent = parent;
            if(parent != null)
            {
                parent.children.Add(com.getComponent<CModuleItem>());
            }
            return com;
        }

        public CFold createFold(CModuleTree parent, string name)
        {
            var com = Component.create<CFold>(new ComponentContainer());
            com.getComponent<CRuntimeObj>().runtime = this;
            com.getComponent<CNamed>()._nameHandle = () => name;
            com.getComponent<CModuleItem>().parent = parent;
            if (parent != null)
            {
                parent.children.Add(com.getComponent<CModuleItem>());
            }
            return com;
        }

        public CIntValue createIntValue(CModuleTree parent, int v)
        {
            var com = Component.create<CIntValue>(new ComponentContainer());
            com.value = v;
            com.getComponent<CRuntimeObj>().runtime = this;
            com.getComponent<CNamed>()._nameHandle = () => com.value.ToString();
            com.getComponent<CModuleItem>().parent = parent;
            if (parent != null)
            {
                parent.children.Add(com.getComponent<CModuleItem>());
            }
            return com;
        }
    }
}