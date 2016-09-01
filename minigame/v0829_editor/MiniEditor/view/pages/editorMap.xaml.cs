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
    /// Interaction logic for editorMap.xaml
    /// </summary>
    public partial class editorMap : UserControl
    {
        TranslateTransform mTranslate = new TranslateTransform();
        ScaleTransform mScale = new ScaleTransform(1, 1);
        public editorMap()
        {
            InitializeComponent();
            TransformGroup group = new TransformGroup();
            group.Children.Add(mScale);
            group.Children.Add(mTranslate);
            
            mCanvas.RenderTransform = group;

            createGrids();
        }


        void createGrids()
        {
            //<Rectangle Width="500" Height="500" Stroke="#FFB6B6B6" Canvas.Left="-250" Canvas.Top="-250"/>
            float startX = -250;
            float startY = -250;
            float space = 50;
            float x = startX;
            float y = startY;
            int lines = 10;
            Brush stroke = new SolidColorBrush(Colors.LightBlue);
            Brush strokeCenter = new SolidColorBrush(Colors.Red);
            for (int xi = 0; xi < lines+1; ++xi)
            {
                x = startX + xi * space;
                Brush s = stroke;
                if(xi == lines/2)
                {
                    s = strokeCenter;
                }
                var line = new Line() { X1 = x, X2 = x, Y1 = startY, Y2 = startY + lines * space, Stroke = s };
                mCanvas.Children.Add(line);
            }
            for (int yi = 0; yi < lines+1; ++yi)
            {
                y = startY + yi * space;
                Brush s = stroke;
                if (yi == lines / 2)
                {
                    s = strokeCenter;
                }
                var line = new Line() { X1 = startX, X2 = startX + lines * space, Y1 = y, Y2 = y, Stroke = s };
                mCanvas.Children.Add(line);
            }


        }
        

        public void setCenter()
        {
            var w = ActualWidth;
            var h = ActualHeight;
            mTranslate.X = w/2;
            mTranslate.Y = h/2;
            //mScale.ScaleY = -1;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            setCenter();
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = Mouse.GetPosition(mCanvas);
            if(e.Delta > 0)
            {
                mScale.ScaleX = mScale.ScaleX * 1.1f;
                mScale.ScaleY = mScale.ScaleY * 1.1f;
                x = p.X * 1.1f;
                y = p.Y * 1.1f;
            }
            else
            {
                mScale.ScaleX = mScale.ScaleX / 1.1f;
                mScale.ScaleY = mScale.ScaleY / 1.1f;
                x = p.X / 1.1f;
                y = p.Y / 1.1f;
            }
            
            mTranslate.X = mTranslate.X - (x - p.X) * mScale.ScaleX;
            mTranslate.Y = mTranslate.Y - (y - p.Y) * mScale.ScaleY;
        }

        double x = 0;
        double y = 0;
        double dx = 0;
        double dy = 0;
        bool mosuePress = false;
        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.MiddleButton == MouseButtonState.Pressed)
            {
                Point p = Mouse.GetPosition(this);
                if (!mosuePress)
                {
                    mosuePress = true;
                    x = p.X;
                    y = p.Y;
                }
                else
                {
                    dx = p.X - x;
                    dy = p.Y - y;
                    mTranslate.X = mTranslate.X + dx;
                    mTranslate.Y = mTranslate.Y + dy;
                    x = p.X;
                    y = p.Y;
                }
            }
            else
            {
                mosuePress = false;
            }
        }
    }
}
