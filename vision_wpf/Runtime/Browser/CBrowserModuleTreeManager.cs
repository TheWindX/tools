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
    class CBrowserModuleTreeManager : Singleton<CBrowserModuleTreeManager>
    {
        UI_browserModuleTree _left;
        UI_browserModuleTree _right;
        Border _leftBorder;
        Border _rightBorder;

        public SolidColorBrush mainFrameBG = new SolidColorBrush(Color.FromArgb(255, 199, 230, 255));
        public SolidColorBrush subFrameBG = new SolidColorBrush(Color.FromArgb(255, 255, 255, 227)); 

        bool left = true;
        public UI_browserModuleTree currentTreeBrowser
        {
            get
            {
                return left ? _left : _right;
            }
        }
        public UI_browserModuleTree otherTreeBrowser
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
            //_left.updateView();
            //_right.updateView();
        }

        public CBrowserModuleTreeManager()
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
            if (kc == System.Windows.Input.Key.Escape)
            {
                chooserName.Clear();
                if (eStatnameChooser == EStatNameChooser.e_seaching)
                {
                    eStatnameChooser = EStatNameChooser.e_stop;
                }
                this.currentTreeBrowser.getMainPanel().m_serachBox.Text = "";
            }
            else if (kc == System.Windows.Input.Key.Enter)
            {
                chooserName.Clear();
                if (eStatnameChooser == EStatNameChooser.e_seaching)
                {
                    eStatnameChooser = EStatNameChooser.e_stop;
                }
                this.currentTreeBrowser.getMainPanel().m_serachBox.Text = "";

                CBrowserModuleTreeManager.Instance.currentTreeBrowser.SetCurrentSpace(null);
            }
            else if (kc >= System.Windows.Input.Key.A && kc <= System.Windows.Input.Key.Z)
            {
                onChar(Convert.ToChar((char)((int)'a' + (int)(kc - System.Windows.Input.Key.A))));
            }
            else if (kc == System.Windows.Input.Key.Back)
            {
                currentTreeBrowser.CDBackView();
            }
            else if (kc == System.Windows.Input.Key.Tab)
            {
                toggle();
            }
            else if(kc == System.Windows.Input.Key.Up || kc == System.Windows.Input.Key.Left)
            {
                var chooseItems = this.currentTreeBrowser.runtimeBrowser.currentSpace.children.Where(
                    item =>
                    {
                        var n = chooserName.ToString();
                        return item.getComponent<CNamed>().name.ToLower().Contains(chooserName.ToString());
                    }).ToList();
                if (chooseItems.Count == 0) return;

                int chooseIdx = -1;
                for (int i = 0; i < chooseItems.Count; ++i)
                {
                    if (chooseItems[i] == this.currentTreeBrowser.runtimeBrowser.selected)
                    {
                        chooseIdx = i - 1;
                        if (chooseIdx < 0)
                        {
                            chooseIdx = chooseItems.Count-1;
                        }
                    }
                }
                if (chooseIdx != -1)
                {
                    var item = chooseItems[chooseIdx];
                    currentTreeBrowser.SetSelect(item);
                }
            }
            else if (kc == System.Windows.Input.Key.Down || kc == System.Windows.Input.Key.Right)
            {
                var chooseItems = this.currentTreeBrowser.runtimeBrowser.currentSpace.children.Where(
                item =>
                {
                    var n = chooserName.ToString();
                    return item.getComponent<CNamed>().name.ToLower().Contains(chooserName.ToString());
                }).ToList();
                if (chooseItems.Count == 0) return;

                int chooseIdx = -1;
                for(int i = 0; i<chooseItems.Count; ++i)
                {
                    if(chooseItems[i] == this.currentTreeBrowser.runtimeBrowser.selected)
                    {
                        chooseIdx = i + 1;
                        if(chooseIdx == chooseItems.Count)
                        {
                            chooseIdx = 0;
                        }
                    }
                }
                if(chooseIdx != -1)
                {
                    var item = chooseItems[chooseIdx];
                    currentTreeBrowser.SetSelect(item);
                }
            }
        }

        public void onChar(char c)
        {
            if(eStatnameChooser == EStatNameChooser.e_stop)
            {
                eStatnameChooser = EStatNameChooser.e_seaching;
            }

            if(eStatnameChooser == EStatNameChooser.e_seaching)
            {
                chooserName.Append(c);
            }
            //filter all name count
            var chooseItems = this.currentTreeBrowser.runtimeBrowser.currentSpace.children.Where(
                item=>
                {
                    var n = chooserName.ToString();
                    return item.getComponent<CNamed>().name.ToLower().Contains(chooserName.ToString());
                }).ToList();
            if (chooseItems.Count == 0) return;
            var mItem = chooseItems.First();

            currentTreeBrowser.SetSelect(mItem);
            this.currentTreeBrowser.getMainPanel().m_serachBox.Text = chooserName.ToString();
        }

        enum EStatNameChooser
        {
            e_stop,
            e_seaching,
        }

        EStatNameChooser eStatnameChooser = EStatNameChooser.e_stop;
        StringBuilder chooserName = new StringBuilder();
        
    }
}
