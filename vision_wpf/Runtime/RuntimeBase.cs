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
    //
    public partial class CNamed : Component
    {
        public System.Func<string> _HandleNamed = () => "noname";
        public string name
        {
            get
            {
                return _HandleNamed();
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
            //override CModuleItem print
            var printer = getComponent<CModuleItem>()._HandlePrint;
            var mi = getComponent<CModuleItem>();
            mi._HandlePrint = space =>
            {
                printer(space);
                foreach (var item in children)
                {
                    item.print(space + 2);
                }
            };
        }
        
        public override void init()
        {
            _HandleDrawUI = () =>
            {
                mUI.runtimeObj = this;
                mUI.clearChildren();

                //增加向上按钮
                var uiUp = new UI_ICON_UP();
                uiUp.setSelect(false);

                mUI.addChild(uiUp);
                foreach (var item in children)
                {
                    item.select(false);
                    var ui = item.drawIcon();
                    mUI.addChild(ui);
                }

                return mUI;
            };
        }
        public static UI_ModuleTree mUI = new UI_ModuleTree();
        
        public System.Func<FrameworkElement> _HandleDrawUI = null;

        public FrameworkElement drawUI()
        {
            return _HandleDrawUI();
        }

        public readonly ListAdvance<CModuleItem> children = new ListAdvance<CModuleItem>();
        
        public void appendChild(CModuleItem node)
        {
            children.Add(node);
        }

        public void setAsCurrent()
        {
            getComponent<CRuntimeObj>().runtime.currentSpace = this;
        }
    }

    public partial class CFold : Component
    {
        public override void inherit()
        {
            addComponent<CModuleTree>();

            //override of CModuleItem._HandleDrawIcon
            var mi = getComponent<CModuleItem>();
            mi._HandleDrawIcon = () =>
            {
                mICon.runtimeObject = this.getComponent<CModuleItem>();
                mICon.setTitle(getComponent<CNamed>().name);
                return mICon;
            };

            //override of CModuleItem._HandleSelect
            mi._HandleSelect = b =>
            {
                mICon.setSelect(b);
            };
        }

        public UI_ICON_Folder mICon = new UI_ICON_Folder();

    }

    public class CModule : Component
    {
        public override void inherit()
        {
            addComponent<CModuleTree>();
            addComponent<CModuleValue>();

            //override of CModuleItem._HandleDrawIcon
            var mi = getComponent<CModuleItem>();
            mi._HandleDrawIcon = () =>
            {
                mICon.runtimeObject = this.getComponent<CModuleItem>();
                mICon.setTitle(getComponent<CNamed>().name);
                return mICon;
            };

            //override of CModuleItem._HandleSelect
            mi._HandleSelect = b =>
            {
                mICon.setSelect(b);
            };
        }

        public UI_ICON_Moduler mICon = new UI_ICON_Moduler();
    }

    public partial class CModuleItem : Component
    {
        public override void inherit()
        {
            addComponent<CNamed>();
        }

        public CModuleTree parent;

        public System.Action<int> _HandlePrint;

        public override void init()
        {
            _HandlePrint = space =>
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < space; ++i)
                    sb.Append(" ");
                sb.Append(this.getComponent<CNamed>().name);
                RuntimeUtil.Instance.log(sb.ToString());
            };

            //override of CModuleItem._HandleDrawIcon
            _HandleDrawIcon = () =>
            {
                mIcon.Text = getComponent<CNamed>().name;
                return mIcon;
            };
            //override of CModuleItem._HandleSelect
            _HandleSelect = b =>
            {
                if (b)
                    mUI.Background = new SolidColorBrush(Colors.AliceBlue);
                else
                    mUI.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            };
        }
        TextBlock mIcon = new TextBlock();

        public void print(int space)
        {   
            _HandlePrint(space);
        }

        public System.Func<FrameworkElement> _HandleDrawIcon;
        TextBlock mUI = new TextBlock();
        public FrameworkElement drawIcon()
        {
            return _HandleDrawIcon();
        }

        public System.Action<bool> _HandleSelect;
        public void select(bool s)
        {
            _HandleSelect(s);
            if(s)
            {
                var rtObj = this.getComponent<CRuntimeObj>();
                rtObj.runtime.selected = this;
            }
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

            //override of CModuleItem._HandleDrawIcon
            var mi = getComponent<CModuleItem>();
            mi._HandleDrawIcon = () =>
            {
                mICon.runtimeObject = this.getComponent<CModuleItem>();
                mICon.setTitle(getComponent<CNamed>().name);
                mICon.setTag("Int");
                return mICon;
            };

            //override of CModuleItem._HandleSelect
            mi._HandleSelect = b =>
            {
                mICon.setSelect(b);
            };
        }

        UI_ICON_BaseTypeValue mICon = new UI_ICON_BaseTypeValue();

        public int value = 0;
    }
}
