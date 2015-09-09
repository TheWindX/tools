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


namespace vision_wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ns_vision.RuntimeUtil.Instance.ShowLogger();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mi = (sender as MenuItem);
            RuntimeUtil.Instance.onTest(mi.Header.ToString());
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            RuntimeUtil.Instance.onKeyUp(e.Key);
        }
    }
}
