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
    }
}
