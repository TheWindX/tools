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
    /// Interaction logic for UI_ICON_Module.xaml
    /// </summary>
    public partial class UI_ICON_Moduler : UI_ICON_ModuleItem
    {
        public UI_ICON_Moduler()
        {
            InitializeComponent();
        }

        protected override TextBlock getTag()
        {
            return null;
        }

        protected override TextBlock getTitle()
        {
            return m_title;
        }

        protected override Rectangle getMask()
        {
            return m_mask;
        }

        protected override void onClick()
        {

        }

        protected override void onDoubleClick()
        {
            var mt = runtimeObject.getComponent<CModuleTree>();
            CBrowserModuleTreeManager.Instance.currentTreeBrowser.SetCurrentSpace(mt);
        }
    }
}
