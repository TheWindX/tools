using ns_vision;
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
    /// Interaction logic for PathViewer.xaml
    /// </summary>
    public partial class PathViewer : UserControl
    {
        public PathViewer()
        {
            InitializeComponent();
        }

        public ModuleTreeBrowser browserView = null;

        CModuleItem _runtimeObject = null;
        public CModuleItem runtimeObject
        {
            get
            {
                return _runtimeObject;
            }
            set
            {
                _runtimeObject = value;
                Stack<CModuleItem> mis = new Stack<CModuleItem>();
                var cur = value;
                while (cur != null)
                {
                    mis.Push(cur);
                    if(cur.parent == null)
                    {
                        cur = null;
                    }
                    else
                    {
                        cur = cur.parent.getComponent<CModuleItem>();
                    }
                }
                m_panel.Children.Clear();
                while (mis.Count>0)
                {
                    var mi = mis.Pop();
                    var bt = new Button();
                    bt.Width = 40;
                    bt.Content = mi.getComponent<CNamed>().name;
                    bt.Click += new RoutedEventHandler((sender, arg) =>
                    {
                        var mt = mi.getComponent<CModuleTree>();
                        if(mt != null)
                        {
                            browserView.SetCurrentSpace(mt);
                        }
                        else
                        {
                            browserView.SetSelect(mi);
                        }
                    });
                    m_panel.Children.Add(bt);
                }
            }
        }
    }
}
