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
    /// Interaction logic for UIMapObj.xaml
    /// </summary>
    public partial class UIMapObj : UserControl
    {
        public UIMapObj()
        {
            InitializeComponent();
        }

        public Brush background
        {
            get
            {
                return mBody.Fill;
            }
            set
            {
                mBody.Fill = value;
            }
        }

        public double radius
        {
            get
            {
                return this.ActualWidth/2;
            }
            set
            {
                this.Width = value*2;
                this.Height = value * 2;
            }
        }

        public double x
        {
            get
            {
                return (double)GetValue(Canvas.LeftProperty)+radius;
            }
            set
            {
                SetValue(Canvas.LeftProperty, value - radius);
            }
        }

        public double y
        {
            get
            {
                return (double)GetValue(Canvas.TopProperty) + radius;
            }
            set
            {
                SetValue(Canvas.TopProperty, value - radius);
            }
        }

        public EditorObject editorObject
        {
            get;set;
        }

        public bool mPicked = false;
        public bool isPicked
        {
            get
            {
                return mPicked;
            }
            set
            {
                mPicked = value;
                if(mPicked)
                {
                    mBody.Stroke = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    mBody.Stroke = null;
                }
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EditorFuncs.instance().getItemListPage().pickEditObject(editorObject);
        }

    }
}
