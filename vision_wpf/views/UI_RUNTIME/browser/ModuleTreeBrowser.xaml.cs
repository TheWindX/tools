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
    /// Interaction logic for ModuleTreeBrowser.xaml
    /// </summary>
    public partial class ModuleTreeBrowser : UserControl
    {
        public ModuleTreeBrowser()
        {
            InitializeComponent();
        }

        public CRuntimeBrowser _runtimeBrowser;
        public CRuntimeBrowser runtimeBrowser
        {
            get
            {
                if(_runtimeBrowser == null)
                {
                    var rt = (App.Current as App).runtime;
                    var bs = rt.createBrowser(rt.rootModule.getComponent<CModuleTree>());
                    _runtimeBrowser = bs;
                }
                
                return _runtimeBrowser;
            }
        }
        
        public void onKeyUp(System.Windows.Input.Key kc)
        {
            if (kc == System.Windows.Input.Key.Back)
            {
                CDBackView();
            }
        }

        public void CDBackView()
        {
            runtimeBrowser.cdback();
            SetCurrentSpace(runtimeBrowser.currentSpace);
        }

        private void setMainPanel(FrameworkElement ui)
        {
            m_panel.Children.Clear();
            m_panel.Children.Add(ui);
        }

        private void SetCurrentSpaceView(CModuleTree tree)
        {
            setMainPanel(tree.drawUI());
        }

        public void SetCurrentSpace(CModuleTree tree)
        {   
            runtimeBrowser.currentSpace = tree;
            runtimeBrowser.selected = null;
            SetCurrentSpaceView(tree);
            m_adress.runtimeObject = tree.getComponent<CModuleItem>();
        }

        public void SetSelect(CModuleItem mi)
        {
            if (mi != null)
            {
                var tree = mi.parent;
                if (tree == null) return;
                runtimeBrowser.currentSpace = tree;
                SetCurrentSpace(tree);
                foreach (var c in tree.children)
                {
                    c.select(false);
                }
                mi.select(true);
            }
        }

        public void updateView()
        {
            setMainPanel(runtimeBrowser.currentSpace.drawUI());
        }
        
    }
}
