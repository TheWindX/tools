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

namespace Touch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //enum e_createLineSt
        //{
        //    e_init,
        //    e_pickOnePoint,
        //    e_pickSecPoint,
        //}
        //e_createLineSt pickSt = e_createLineSt.e_init;
        //CNodeLink curLink = null;

        public MainWindow()
        {
            InitializeComponent();

            //m_node1.evtOnRightDown += pt =>
            //{
            //    if(pickSt == e_createLineSt.e_init)
            //    {
            //        pickSt = e_createLineSt.e_pickOnePoint;
            //        curLink = new CNodeLink();
            //        curLink.IsEnabled = false;
            //        curLink.IsHitTestVisible = false;
            //        m_panel.Children.Add(curLink);
            //        curLink.leftNode = m_node1;
            //        curLink.begin = pt;
            //    }
            //};

            //m_node2.evtOnLeftUp += pt =>
            //{
            //    if (pickSt == e_createLineSt.e_pickOnePoint)
            //    { 
            //        pickSt = e_createLineSt.e_init;
            //        curLink.end = pt;
            //        curLink.rightNode = m_node2;
            //    }
            //};

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var ins = entityManager.ins;
        }

        //private void m_panel_PreviewMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (pickSt == e_createLineSt.e_pickOnePoint)
        //    {
        //        curLink.end = e.GetPosition(this);
        //        this.UpdateLayout();
        //    }
        //}


    }
}
