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
    /// Interaction logic for ComponentPanel.xaml
    /// </summary>
    public partial class ComponentPanel : UserControl
    {
        #region props
        public MComponent component
        {
            get
            {
                return mCom;
            }
            set
            {
                mCom = value;
                reflush();
            }
        }

        #endregion

        public ComponentPanel()
        {
            InitializeComponent();
        }

        private MComponent mCom = null;

        public ComponentCustomAttribute getAttr()
        {
            var attr = mCom.GetType().GetCustomAttribute<ComponentCustomAttribute>();
            return attr;
        }

        void reflush()
        {
            var attr = getAttr();
            if(attr!=null)
            {
                mName.Content = attr.name;
                EditorWorld.addTypeControl(mCom, mProps);
                if(!attr.removable)
                {
                    mHead.IsEnabled = false;
                }
                //if (!attr.isMain)
                //{
                //    mHead.IsEnabled = false;
                //}
                if (!attr.editable)
                {
                    mProps.IsEnabled = false;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //弹出右键菜单
            var mContextMenu = new ContextMenu();
            var coms = EditorWorld.getAssemblyModules();
            MenuItem mi = new MenuItem();
            mi.Header = "remove";
            mContextMenu.Items.Add(mi);
            mi.Click += new RoutedEventHandler((obj, arg) =>
            {
                var editObj = EditorFuncs.instance().getItemListPage().getCurrentObj();
                editObj.removeComponent(component.GetType());
                EditorFuncs.instance().getComponentPage().reflush();
            });
            mContextMenu.IsOpen = true;
        }
    }
}
