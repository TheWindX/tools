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
    /// WindowStaus.xaml 的交互逻辑
    /// </summary>
    public partial class WindowStaus : Window
    {
        public WindowStaus()
        {
            InitializeComponent();
        }

        public List<string> m_infos = new List<string>();
        public List<string> m_errors = new List<string>();

        public void addInfo(string info)
        {
            m_infos.Add(info);
            //var lb = new TextBox();
            //lb.Text = info;
            //lb.BorderThickness = new Thickness(0);
            //lb.IsReadOnly = true;
            //lb.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            //m_infoview.Children.Add(lb);

            m_infotext.Text = m_infos.Aggregate("", (acc, item) =>
            {
                return acc + item + "\n";
            });
        }

        public void addError(string error)
        {
            m_errors.Add(error);
            //var lb = new TextBox();
            //lb.Text = error;
            //lb.BorderThickness = new Thickness(0);
            //lb.IsReadOnly = true;
            //lb.Foreground = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0));
            //lb.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            //m_errorview.Children.Add(lb);
            m_errtext.Text = m_errors.Aggregate("", (acc, item) =>
            {
                return acc + item + "\n";
            });
        }

        public event System.Action evtClear;
        public void clear()
        {
            m_infos.Clear();
            m_errors.Clear();
            //m_infoview.Children.Clear();
            //m_errorview.Children.Clear();
            m_infotext.Text = "";
            m_errtext.Text = "";
            if (evtClear != null) evtClear();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
