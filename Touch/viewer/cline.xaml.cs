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

namespace Touch
{
    /// <summary>
    /// Interaction logic for Line.xaml
    /// </summary>
    public partial class CLink : UserControl
    {
        public CLink()
        {
            InitializeComponent();
        }
        
        public Point begin
        {
            get
            {
                return (Point)GetValue(beginProp);
            }
            set
            {
                SetValue(beginProp, value);
            }
        }


        public Point end
        {
            get
            {
                return (Point)GetValue(endProp);
            }
            set
            {
                SetValue(endProp, value);
            }
        }


        static DependencyProperty beginProp = DependencyProperty.Register("begin", typeof(Point), typeof(CLink), new PropertyMetadata(new Point(0, 0), dataChanged));
        static DependencyProperty endProp = DependencyProperty.Register("end", typeof(Point), typeof(CLink), new PropertyMetadata(new Point(0, 0), dataChanged));

        public static void dataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CLink).update();
        }

        public void update()
        {
            SetValue(Grid.MarginProperty, new Thickness(begin.X, begin.Y, 0, 0));
            m_seg.Point3 = new Point(end.X - begin.X, end.Y - begin.Y);
            m_seg.Point1 = new Point(64, 0);
            m_seg.Point2 = new Point(m_seg.Point3.X - 64, m_seg.Point3.Y);
        }
    }
}
