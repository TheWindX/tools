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
    /// Interaction logic for Discription.xaml
    /// </summary>
    public partial class DiscriptionCtrl : UserControl
    {
        public DiscriptionCtrl()
        {
            InitializeComponent();
        }

        public string text
        {
            get
            {
                return mContent.Text;
            }
            set
            {
                mContent.Text = value;
            }
        }
    }
}
