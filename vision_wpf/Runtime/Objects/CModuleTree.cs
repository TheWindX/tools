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
                mUI = new UI_Panel_ModuleTree();
                mUI.runtimeObj = this;
                mUI.clearChildren();

                //增加向上按钮
                //var uiUp = new UI_ICON_UP();
                //mUI.addChild(uiUp);

                foreach (var item in children)
                {
                    item.select(false);
                    var ui = item.drawIcon();
                    mUI.addChild(ui as UI_ICON_ModuleItem);
                }

                return mUI;
            };
        }

        public static UI_Panel_ModuleTree mUI = null;

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

        
        public CModule createModule(string name)
        {
            var com = Component.create<CModule>(new ComponentContainer());
            com.getComponent<CNamed>()._HandleNamed = () => name;
            com.getComponent<CRuntimeObj>().runtime = this.getComponent<CRuntimeObj>().runtime;
            com.getComponent<CModuleItem>().parent = this;
            children.Add(com.getComponent<CModuleItem>());
            return com;
        }

        public CModuleFold createFold(string name)
        {
            var com = Component.create<CModuleFold>(new ComponentContainer());
            com.getComponent<CRuntimeObj>().runtime = this.getComponent<CRuntimeObj>().runtime;
            com.getComponent<CNamed>()._HandleNamed = () => name;
            com.getComponent<CModuleItem>().parent = this;
            this.children.Add(com.getComponent<CModuleItem>());
            return com;
        }

        public CValueInt createIntValue(int v)
        {
            var com = Component.create<CValueInt>(new ComponentContainer());
            com.value = v;
            com.getComponent<CRuntimeObj>().runtime = this.getComponent<CRuntimeObj>().runtime;
            com.getComponent<CNamed>()._HandleNamed = () => com.value.ToString();
            com.getComponent<CModuleItem>().parent = this;
            this.children.Add(com.getComponent<CModuleItem>());
            return com;
        }

        
    }

}