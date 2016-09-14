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
    /// Interaction logic for LableBreak.xaml
    /// </summary>
    public partial class LableBreak : UserControl
    {
        public LableBreak(Panel parent, string lable)
        {
            InitializeComponent();
            Lable = lable;
            parent.Children.Add(this);
        }

        public string Lable
        {
            get
            {
                return m_label.Text;
            }
            set
            {
                m_label.Text = value;
            }
        }
    }
}
