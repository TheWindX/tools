using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace ns_vision
{
    class CRuntimeBrowserViewManager : Singleton<CRuntimeBrowserViewManager>
    {
        ModuleTreeBrowser _left;
        ModuleTreeBrowser _right;
        Border _leftBorder;
        Border _rightBorder;

        public SolidColorBrush mainFrameBG = new SolidColorBrush(Color.FromArgb(255, 199, 230, 255));
        public SolidColorBrush subFrameBG = new SolidColorBrush(Color.FromArgb(255, 255, 255, 227)); 

        bool left = true;
        public ModuleTreeBrowser currentTreeBrowser
        {
            get
            {
                return left ? _left : _right;
            }
        }
        public ModuleTreeBrowser otherTreeBrowser
        {
            get
            {
                return left ? _right : _left;
            }
        }

        public void toggle()
        {
            left = !left;
            updateView();
        }

        private void updateView()
        {
            if (left)
            {
                _leftBorder.Background = mainFrameBG;
                _rightBorder.Background = subFrameBG;
            }
            else
            {
                _leftBorder.Background = subFrameBG;
                _rightBorder.Background = mainFrameBG;
            }
        }

        public CRuntimeBrowserViewManager()
        {   
        }

        public void init()
        {   
            _left = (App.Current.MainWindow as MainWindow).m_leftBrowser;
            _right = (App.Current.MainWindow as MainWindow).m_rightBrowser;
            _leftBorder = (App.Current.MainWindow as MainWindow).m_leftBrowserFrame;
            _rightBorder = (App.Current.MainWindow as MainWindow).m_rightBrowserFrame;

            left = true;
            updateView();
            
            //key binding
            RuntimeUtil.Instance.getMainWindow().evtOnKey += onKeyUp;
        }

        public void onKeyUp(System.Windows.Input.Key kc)
        {
            if (kc == System.Windows.Input.Key.Back)
            {
                currentTreeBrowser.CDBackView();
            }
            else if (kc == System.Windows.Input.Key.Tab)
            {
                toggle();
            }
        }
    }
}
