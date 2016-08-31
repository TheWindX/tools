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
        public EditObject editObject
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

        static List<Type> getAssemblyModules()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            var ts = myAssembly.GetTypes();

            List<Type> mComponents = new List<Type>();
            foreach (var t in ts)
            {
                if (typeof(MComponent).IsAssignableFrom(t))
                {
                    var attrs = t.GetCustomAttribute<ModuleCustomAttribute>();
                    if (attrs != null)
                    {
                        //MModule instance = (MModule)Activator.CreateInstance(t);
                        mComponents.Add(t);
                    }
                }
            }
            return mComponents;
        }

        private void mAddComponent_Click(object sender, RoutedEventArgs e)
        {
            //弹出右键菜单
            var mContextMenu = new ContextMenu();
            var coms = getAssemblyModules();
            foreach(var com in coms)
            {
                var comCopy = com;
                MenuItem mi = new MenuItem();
                mi.Header = comCopy.Name;
                mContextMenu.Items.Add(mi);
                mi.Click += new RoutedEventHandler((obj, arg) =>
                {
                    var editObj = EditorFuncs.instance().getItemListPage().getCurrentObj();
                    if (editObj == null) return;
                    editObj.addComponent(comCopy);
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
            foreach (var com in editObject.components)
            {
                var comp = new ComponentPanel() { component = com };
                mComponents.Children.Add(comp);
            }
        }
    }
}
