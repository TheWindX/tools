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
    /// Interaction logic for node.xaml
    /// </summary>
    public partial class CNode : UserControl
    {
        public CNode()
        {
            InitializeComponent();
        }

        public string text
        {
            get
            {
                return m_text.Text;
            }
            set
            {
                m_text.Text = value;
            }
        }
        
        Point mStartPoint;
        Point mStartMousePoint;
        bool drag = false;
        private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focusable = true;
            Keyboard.Focus(this);
            if(Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                return;
            }
            
            mStartPoint.X = Margin.Left;
            mStartPoint.Y = Margin.Top;
            mStartMousePoint.X = Mouse.GetPosition(this.Parent as IInputElement).X;
            mStartMousePoint.Y = Mouse.GetPosition(this.Parent as IInputElement).Y;
            drag = true;
            this.CaptureMouse();

            if(evtSelect != null)
            {
                evtSelect();
            }
        }

        public System.Action evtSelect;

        private void UserControl_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if(drag)
            {
                var x = Mouse.GetPosition(this.Parent as IInputElement).X;
                var y = Mouse.GetPosition(this.Parent as IInputElement).Y;
                var delta = new Point(x - mStartMousePoint.X, y - mStartMousePoint.Y);
                Console.WriteLine(delta);
                onDrag(delta, new Point(mStartPoint.X + delta.X, mStartPoint.Y + delta.Y));
            }
        }

        private void MUI_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2)
            {
                StringEnter.ShowOnTop(str =>
                {
                    text = str;
                });
            }
        }

        void onDrag(Point deltaPos, Point newThisPos)
        {
            Margin = new Thickness(newThisPos.X, newThisPos.Y, 0, 0);
            if(evtOnDrag != null)
            {
                evtOnDrag();
            }
        }
        public System.Action evtOnDrag;

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drag = false;
            this.ReleaseMouseCapture();
        }
        
        public Point leftPos
        {
            get
            {
                return m_left.TransformToAncestor(this.Parent as Visual).Transform(new Point(m_right.Width / 2, m_right.Height / 2));
            }
        }

        public Point rightPos
        {
            get
            {
                return m_right.TransformToAncestor(this.Parent as Visual).Transform(new Point(m_right.Width / 2, m_right.Height / 2));
            }
        }

        public System.Action<Point> evtOnRightDown;
        public System.Action<Point> evtOnLeftUp;
        private void m_right_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Point relativePoint = m_right.TransformToAncestor(this.Parent as Visual).Transform(new Point(m_right.Width / 2, m_right.Height / 2));
                if (evtOnRightDown != null)
                {
                    evtOnRightDown(relativePoint);
                }
                return;
            }
        }

        private void m_left_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Point relativePoint = m_left.TransformToAncestor(this.Parent as Visual).Transform(new Point(m_right.Width / 2, m_right.Height / 2));
                if (evtOnLeftUp != null)
                {
                    evtOnLeftUp(relativePoint);
                }
                return;
            }
        }
    }
}
