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
using System.Windows.Shapes;

namespace MiniEditor
{
    /// <summary>
    /// Interaction logic for WindowsF1.xaml
    /// </summary>
    public partial class WindowsF1 : Window
    {
        public WindowsF1()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            clear();
            init();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = System.Windows.Visibility.Hidden;

        }

        public void init()
        {
            addItem("F1:帮助");
            addItem("INSERT:测试增加子物件");
        }

        public void clear()
        {
            m_items.Children.Clear();
        }

        public void addItem(string strItem)
        {
            //if(controlParttern == null)
            //{
            //    controlParttern = XamlWriter.Save(m_item_template);
            //}
            var newLb = WPFUtil.XamlClone(m_item_template);
            newLb.Content = strItem;
            newLb.Visibility = System.Windows.Visibility.Visible;
            m_items.Children.Add(newLb);
        }

    }
}
