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

namespace Touch
{
    /// <summary>
    /// Interaction logic for StringEnter.xaml
    /// </summary>
    public partial class StringEnter : Window
    {
        public StringEnter()
        {
            InitializeComponent();
            Owner = App.Current.MainWindow;
        }

        public System.Action<string> evtEnter = null;

        static StringEnter mEtner = null;
        public static void ShowOnTop(System.Action<string> handler)
        {
            if (mEtner == null)
            {
                mEtner = new StringEnter();
            }
            mEtner.evtEnter = handler;
            mEtner.m_text.Clear();

            mEtner.Left = Mouse.GetPosition(mEtner.Owner).X;
            mEtner.Top = Mouse.GetPosition(mEtner.Owner).Y;

            mEtner.Show();
            mEtner.Topmost = true;
            mEtner.Activate();
            mEtner.m_text.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (evtEnter != null)
            {
                evtEnter(m_text.Text);
                this.Hide();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (evtEnter != null)
                {
                    evtEnter(m_text.Text);
                    this.Hide();
                }
            }
        }
    }
}
