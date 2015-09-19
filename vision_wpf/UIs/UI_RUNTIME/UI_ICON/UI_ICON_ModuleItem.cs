using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ns_vision
{
    /// <summary>
    /// Interaction logic for UI_BaseTypeValue.xaml
    /// </summary>
    public class UI_ICON_ModuleItem : UserControl
    {
        public UI_ICON_ModuleItem()
        {
            PreviewMouseLeftButtonDown += onMouseLeftButtonDown;
            Loaded += UI_ICON_Loaded;
        }

        private void UI_ICON_Loaded(object sender, RoutedEventArgs e)
        {
            setSelect(false);
        }

        protected virtual TextBlock getTag()
        {
            return null;
        }

        protected virtual TextBlock getTitle()
        {
            return null;
        }

        protected virtual Rectangle getMask()
        {
            return null;
        }

        protected virtual void onClick()
        {

        }

        protected virtual void onDoubleClick()
        {

        }

        public void setTag(string tag)
        {
            if(getTag() != null)
            {
                getTag().Text = tag;
            }
        }

        public void setTitle(string name)
        {
            if (getTitle() != null)
            {
                getTitle().Text = name;
            }
        }

        public void setSelect(bool b)
        {
            if (getMask() == null) return;
            var mask = getMask();

            if (b)
            {
                var cb = (mask.Fill as SolidColorBrush);
                cb.Color = Color.FromArgb(40, cb.Color.R, cb.Color.G, cb.Color.B);
            }
            else
            {
                var cb = (mask.Fill as SolidColorBrush);
                cb.Color = Color.FromArgb(1, cb.Color.R, cb.Color.G, cb.Color.B);
            }
        }

        
        public CModuleItem runtimeObject
        {
            get;
            set;
        }

        public UI_browserModuleTree browser
        {
            get;
            set;
        }

        private void onMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 1)
            {
                var p = (Parent as Panel);
                foreach(var item in p.Children)
                {
                    var icon = (item as UI_ICON_ModuleItem);
                    if(icon != null)
                    {
                        icon.setSelect(false);
                    }
                }
                setSelect(true);
                onClick();
            }
            else if (e.ClickCount == 2)
            {
                onDoubleClick();
            }
        }
    }
}
