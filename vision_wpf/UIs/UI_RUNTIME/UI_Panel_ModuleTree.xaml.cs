using System;
using System.Collections;
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
    public partial class UI_Panel_ModuleTree : UserControl, IEnumerable<UI_ICON_ModuleItem>
    {
        public UI_Panel_ModuleTree()
        {
            InitializeComponent();
        }

        public void addChild(UI_ICON_ModuleItem child)
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
                                    var bsView = CBrowserModuleTreeManager.Instance.currentTreeBrowser;
                                    bsView.runtimeBrowser.currentSpace.createFold(enterString);
                                    bsView.updateView();
                                });
                            }
                            else if (cmd == "new module")
                            {
                                RuntimeUtil.Instance.popStringEnter(enterString =>
                                {
                                    var ro = runtimeObj.getComponent<CRuntimeObj>();
                                    var bsView = CBrowserModuleTreeManager.Instance.currentTreeBrowser;
                                    bsView.runtimeBrowser.currentSpace.createModule(enterString);
                                    bsView.updateView();
                                });
                            }
                            else if (cmd == "new int value")
                            {
                                RuntimeUtil.Instance.popStringEnter(enterString =>
                                {
                                    var ro = runtimeObj.getComponent<CRuntimeObj>();
                                    int val = -1;
                                    bool b = int.TryParse(enterString, out val);
                                    var bsView = CBrowserModuleTreeManager.Instance.currentTreeBrowser;
                                    bsView.runtimeBrowser.currentSpace.createIntValue(val);
                                    bsView.updateView();
                                });
                            }
                        });
                }
            }
        }

        private void m_panel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var mtx = RenderTransform.Value;
            if(e.Delta > 0)
            {
                mtx.Scale(0.9, 0.9);
            }
            else
            {
                mtx.Scale(1.1, 1.1);
            }
            
            RenderTransform = new MatrixTransform(mtx);
        }

        private IEnumerable<UI_ICON_ModuleItem> getEnumerable()
        {
            foreach (var item in m_panel.Children)
            {
                if (item is UI_ICON_ModuleItem)
                {
                    yield return item as UI_ICON_ModuleItem;
                }
            }
        }

        public IEnumerator<UI_ICON_ModuleItem> GetEnumerator()
        {
            return getEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return getEnumerable().GetEnumerator();
        }
    }
}
