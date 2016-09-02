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
                return EditorFuncs.instance().getItemListPage().getCurrentObj();
            }
        }
        #endregion

        public ComponentPanelList()
        {
            InitializeComponent();
        }

        private void mAddComponent_Click(object sender, RoutedEventArgs e)
        {
            //弹出右键菜单
            var mContextMenu = new ContextMenu();
            var coms = EditorWorld.getAssemblyComponents();
            foreach(var com in coms)
            {
                var comCopy = com;
                var attr = comCopy.GetCustomAttribute<CustomComponentAttribute>();
                MenuItem mi = new MenuItem();
                mi.Header = attr.name;
                mContextMenu.Items.Add(mi);
                mi.Click += new RoutedEventHandler((obj, arg) =>
                {
                    var editObj = EditorFuncs.instance().getItemListPage().getCurrentObj();
                    if (editObj == null) return;
                    var comInstances = editObj.addComponent(comCopy);
                    foreach(var comInstance in comInstances)
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
