using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ns_vision
{
    //
    public partial class CNamed : Component
    {
        public System.Func<string> _nameHandle = () => "noname";
        public string name
        {
            get
            {
                return _nameHandle();
            }
        }

        public override void inherit()
        {
            addComponent<CRuntimeObj>();
        }
    }

    public partial class CRuntimeObj : Component
    {
        public CRuntime runtime;
        public override void inherit()
        {
            //addComponent<CNamed>();
        }
    }

    public partial class CRuntime : Component
    {
        public CModule rootModule { get; private set; }
        public override void init()
        {
            rootModule = createModule(null, "root");
        }
    }

    //可作为路径的元素
    public partial class CModuleTree : Component
    {
        public override void inherit()
        {
            addComponent<CModuleItem>();
        }

        public override void init()
        {
            //override CModuleItem print
            var printer = getComponent<CModuleItem>()._HandlePrint;
            getComponent<CModuleItem>()._HandlePrint = space =>
            {
                printer(space);
                foreach(var item in children)
                {
                    item.print(space+2);
                }
            };
        }
        
        public readonly ListAdvance<CModuleItem> children = new ListAdvance<CModuleItem>();
        
        public void appendChild(CModuleItem node)
        {
            children.Add(node);
        }
    }

    public partial class CFold : Component
    {
        public override void inherit()
        {
            addComponent<CModuleTree>();
        }
    }

    public class CModule : Component
    {
        public override void inherit()
        {
            addComponent<CModuleTree>();
            addComponent<CModuleValue>();
        }
    }

    public partial class CModuleItem : Component
    {
        public CModuleTree parent;

        public System.Action<int> _HandlePrint;

        public CModuleItem()
        {
            _HandlePrint = space =>
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < space; ++i)
                    sb.Append(" ");
                sb.Append(this.getComponent<CNamed>().name);
                RuntimeUtil.Instance.log(sb.ToString());
            };
        }

        public void print(int space)
        {   
            _HandlePrint(space);
        }
        public override void inherit()
        {
            addComponent<CNamed>();
        }
    }

    public partial class CModuleValue : Component
    {
        public override void inherit()
        {
            addComponent<CModuleItem>();
        }
    }
    
    public partial class CIntValue : Component
    {
        public override void inherit()
        {
            addComponent<CModuleValue>();
        }

        public int value = 0;
    }
}
