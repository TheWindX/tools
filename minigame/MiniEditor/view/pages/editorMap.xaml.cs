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
            Brush stroke = new SolidColorBrush(Color.FromArgb(200, 102, 102, 153));
            Brush strokeCenter = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0));
            for (int xi = 0; xi < lines + 1; ++xi)
            {
                x = startX + xi * space;
                Brush s = stroke;
                if (xi == lines / 2)
                {
                    s = strokeCenter;
                }
                var line = new Line() { X1 = x, X2 = x, Y1 = startY, Y2 = startY + lines * space, Stroke = s };
                mCanvas.Children.Add(line);
            }
            for (int yi = 0; yi < lines + 1; ++yi)
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
            mTranslate.X = w / 2;
            mTranslate.Y = h / 2;
            mScale.ScaleY = -mScale.ScaleX;

        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            setCenter();
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = EditorFuncs.getMousePositionInMap();

            double x = 0;
            double y = 0;
            var oldScale = mScale.ScaleX;
            if (e.Delta > 0)
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

        public void addItem(FrameworkElement obj)
        {
            mCanvas.Children.Add(obj);
        }

        public void removeItem(FrameworkElement obj)
        {
            mCanvas.Children.Remove(obj);
        }

        public void clear(UIMapObj obj)
        {
            mCanvas.Children.Clear();
        }
        
        Point mCurrentGridPoint = new Point(0, 0);
        Point mCurrentCanvasPoint = new Point(0, 0);
        bool mIsLeftDown = false;
        bool mIsMiddleDown = false;
        bool mIsRightDown = false;


        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mCurrentGridPoint = EditorFuncs.getMousePositionInMapWindow();
            mCurrentCanvasPoint = EditorFuncs.getMousePositionInMap();
            if (e.ChangedButton == MouseButton.Left)
            {
                mIsLeftDown = true;
                EditorFuncs.doLeftMouseDown(mCurrentCanvasPoint.X, mCurrentCanvasPoint.Y);
            }
            else if (e.ChangedButton == MouseButton.Middle)
            {
                mIsMiddleDown = true;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                mIsRightDown = true;
                EditorFuncs.doRightMouseDown(mCurrentCanvasPoint.X, mCurrentCanvasPoint.Y);
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point p = EditorFuncs.getMousePositionInMap();
            if (e.ChangedButton == MouseButton.Left)
            {
                mIsLeftDown = false;
                EditorFuncs.doLeftMouseUp(p.X, p.Y);
            }
            else if (e.ChangedButton == MouseButton.Middle)
            {
                mIsMiddleDown = false;
            }
            else if (e.ChangedButton == MouseButton.Right)
            {
                mIsRightDown = false;
                EditorFuncs.doRightMouseUp(p.X, p.Y);
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = EditorFuncs.getMousePositionInMap();
            Point pGrid = EditorFuncs.getMousePositionInMapWindow();
            var dx = p.X - mCurrentCanvasPoint.X;
            var dy = p.Y - mCurrentCanvasPoint.Y;
            var dxWindow = pGrid.X - mCurrentGridPoint.X;
            var dyWindow = pGrid.Y - mCurrentGridPoint.Y;
            mCurrentCanvasPoint = p;
            mCurrentGridPoint = pGrid;
            if (mIsLeftDown)
            {
                EditorFuncs.doLeftMouseDrag(dx, dy);
            }
            else if(mIsMiddleDown)
            {
                mTranslate.X = mTranslate.X + dxWindow;
                mTranslate.Y = mTranslate.Y + dyWindow;
            }
            else if (mIsRightDown)
            {
                EditorFuncs.doRightMouseDrag(dx, dy);
            }
        }
    }
}
