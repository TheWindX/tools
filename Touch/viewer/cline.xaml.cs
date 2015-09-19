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

        Point _start;
        public Point start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
                update();
            }
        }

        public void update()
        {
            SetValue(Grid.MarginProperty, new Thickness(_start.X, _start.Y, 0, 0));
            m_seg.Point3 = new Point(_end.X - _start.X, _end.Y - _start.Y);
            m_seg.Point1 = new Point(64, 0);
            m_seg.Point2 = new Point(m_seg.Point3.X - 64, m_seg.Point3.Y);
        }

        Point _end;
        public Point end
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                update();
            }
        }

    }
}
