using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ns_vision
{
    /// <summary>
    /// Interaction logic for UI_ModuleTree.xaml
    /// </summary>
    public partial class UI_ModuleTree : UserControl
    {
        public UI_ModuleTree()
        {
            InitializeComponent();
        }

        public void addChild(FrameworkElement child)
        {
            m_panel.Children.Add(child);
        }

        public void clearChildren()
        {
            m_panel.Children.Clear();
        }

        public void setTitle(string title)
        {
            m_title.Text = title;
        }

        public CModuleTree runtimeObj
        {
            get;
            set;
        }

        private void m_panel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    RuntimeUtil.Instance.popupContext(new List<string>() { "new fold", "new module", "new int value" },
                        cmd =>
                        {
                            if (cmd == "new fold")
                            {
                                RuntimeUtil.Instance.popStringEnter(enterString =>
                                {
                                    var ro = runtimeObj.getComponent<CRuntimeObj>();
                                    ro.runtime.createFold(runtimeObj, enterString);
                                    RuntimeUtil.Instance.updateView();
                                });
                            }
                            else if (cmd == "new module")
                            {
                                RuntimeUtil.Instance.popStringEnter(enterString =>
                                {
                                    var ro = runtimeObj.getComponent<CRuntimeObj>();
                                    ro.runtime.createModule(runtimeObj, enterString);
                                    RuntimeUtil.Instance.updateView();
                                });
                            }
                            else if (cmd == "new int value")
                            {
                                RuntimeUtil.Instance.popStringEnter(enterString =>
                                {
                                    var ro = runtimeObj.getComponent<CRuntimeObj>();
                                    int val = -1;
                                    bool b = int.TryParse(enterString, out val);
                                    ro.runtime.createIntValue(runtimeObj, val);
                                    RuntimeUtil.Instance.updateView();
                                });
                            }
                        });
                }
            }
        }
    }
}
