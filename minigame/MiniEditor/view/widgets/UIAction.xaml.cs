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

namespace MiniEditor
{
    /// <summary>
    /// Interaction logic for UIAction.xaml
    /// </summary>
    public partial class UIAction : UserControl
    {
        public UIAction()
        {
            InitializeComponent();
        }

        public Action mVal = null;
        public Action Val
        {
            set
            {
                mVal = value;
                m_value.Click += M_value_Click;
            }
        }

        private void M_value_Click(object sender, RoutedEventArgs e)
        {
            if (mVal != null) mVal();
        }

        public string Lable
        {
            get
            {
                return (string)m_value.Content;
            }
            set
            {
                m_value.Content = value;
            }
        }
    }
}
