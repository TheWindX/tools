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
using System.Windows.Shapes;

namespace vision_wpf.views
{
    /// <summary>
    /// Interaction logic for WindowLogger.xaml
    /// </summary>
    public partial class WindowLogger : Window
    {
        public WindowLogger()
        {
            InitializeComponent();
            Owner = App.Current.MainWindow;
        }

        public List<string> m_infos = new List<string>();
        public List<string> m_errors = new List<string>();

        public void addInfo(string info)
        {
            m_infos.Add(info);
            m_infotext.Text = m_infos.Aggregate("", (acc, item) =>
            {
                return acc + item + "\n";
            });
        }

        public void addError(string error)
        {
            m_errors.Add(error);
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

        public void showAtCenter()
        {
            Show();
            Left = Owner.Left + (Owner.Width - Owner.ActualWidth) / 2;
            Top = Owner.Top + (Owner.Height - Owner.ActualHeight) / 2;
            Activate();
        }
    }
}
