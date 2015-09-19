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
            mStartPoint.X = Margin.Left;
            mStartPoint.Y = Margin.Top;
            mStartMousePoint.X = Mouse.GetPosition(this.Parent as IInputElement).X;
            mStartMousePoint.Y = Mouse.GetPosition(this.Parent as IInputElement).Y;
            drag = true;
            this.CaptureMouse();
        }

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

        public void onDrag(Point deltaPos, Point newThisPos)
        {
            Margin = new Thickness(newThisPos.X, newThisPos.Y, 0, 0);
        }

        private void UserControl_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drag = false;
            this.ReleaseMouseCapture();
        }
    }
}
