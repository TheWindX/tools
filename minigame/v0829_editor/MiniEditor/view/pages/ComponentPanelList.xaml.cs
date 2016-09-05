using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace MiniEditor
{
    /// <summary>
    /// Interaction logic for ComponentPanelList.xaml
    /// </summary>
    public partial class ComponentPanelList : UserControl
    {
        #region props
        public EditorObject editObject
        {
            get
            {
                return EditorFuncs.getItemListPage().getCurrentObj();
            }
        }
        #endregion

        public ComponentPanelList()
        {
            InitializeComponent();
        }

        private void AddMenuItem(ItemsControl itemUI, IEnumerable<RepoNode> items)
        {
            foreach (var item in items)
            {
                if (item is RepoBranch)
                {
                    var b = item as RepoBranch;
                    var mi = new MenuItem();
                    mi.Header = b.name;
                    AddMenuItem(mi, b.children);
                    itemUI.Items.Add(mi);
                }
                else if (item is RepoLeaf)
                {
                    var l = item as RepoLeaf;
                    var comType = l.component;
                    var attr = comType.GetCustomAttribute<CustomComponentAttribute>();
                    MenuItem mi = new MenuItem();
                    mi.Header = attr.name;
                    itemUI.Items.Add(mi);
                    mi.Click += new RoutedEventHandler((obj, arg) =>
                    {
                        var editObj = EditorFuncs.getItemListPage().getCurrentObj();
                        if (editObj == null) return;
                        var comInstances = editObj.addComponent(comType);
                        foreach (var comInstance in comInstances)
                        {
                            try
                            {
                                comInstance.editorInit();
                            }
                            catch (Exception ex)
                            {
                                MLogger.error(ex.ToString());
                            }
                        }
                        reflush();
                    });
                }
            }
        }

        private void mAddComponent_Click(object sender, RoutedEventArgs e)
        {
            var mContextMenu = new ContextMenu();
            var items = ComponentRepository.subItems;
            AddMenuItem(mContextMenu, items);
            mContextMenu.IsOpen = true;
        }

        //这里刷新prop显示
        public void reflush()
        {
            if (editObject == null) return;
            mComponents.Children.Clear();
            mObjectName.Text = editObject.name;
            List<ComponentPanel> panels = new List<ComponentPanel>();
            ComponentPanel mainPanel = null;
            foreach (var com in editObject.components)
            {
                var comP = new ComponentPanel() { component = com };
                if(comP.getAttr().isMain)
                {
                    mainPanel = comP;
                }
                else
                {
                    panels.Add(comP);
                }
            }
            if(mainPanel != null)
            {
                mComponents.Children.Add(mainPanel);
            }
            foreach (var p in panels)
            {
                mComponents.Children.Add(p);
            }
        }
    }
}
