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
    /// Interaction logic for mapObjectTarget.xaml
    /// </summary>
    public partial class UIMapObjectTarget : UserControl
    {
        public UIMapObjectTarget()
        {
            InitializeComponent();
        }

        public double size
        {
            get
            {
                return this.Width;
            }
            set
            {
                this.Width = value;
                this.Height = value;
            }
        }

        public double x
        {
            get
            {
                return (double)GetValue(Canvas.LeftProperty) + size/2;
            }
            set
            {
                SetValue(Canvas.LeftProperty, value - size/2);
            }
        }

        public double y
        {
            get
            {
                return (double)GetValue(Canvas.TopProperty) + size / 2;
            }
            set
            {
                SetValue(Canvas.TopProperty, value - size / 2);
            }
        }

        public EditorObject editorObject
        {
            get; set;
        }

    }
}
