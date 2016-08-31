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
    /// Interaction logic for stringField.xaml
    /// </summary>
    public partial class stringField : UserControl
    {
        public stringField()
        {
            InitializeComponent();
            m_value.TextChanged += hValueChange;
        }

        void hValueChange(object sender, RoutedEventArgs e)
        {
            if (evtValueChanged != null) evtValueChanged();
        }

        void hLostFocus(object sender, RoutedEventArgs e)
        {
            if (evtFocusLost != null) evtFocusLost();
        }
        public event System.Action evtValueChanged;
        public event System.Action evtFocusLost;

        public string Val
        {
            get
            {
                return m_value.Text;
            }
            set
            {
                m_value.Text = value;
            }
        }

        public string Lable
        {
            get
            {
                return m_label.Text;
            }
            set
            {
                m_label.Text = value;
            }
        }

        System.Func<List<string>> _Chooser = null;
        System.Action<System.Action<List<string>>> _ChooseContinue = null;

        public System.Func<List<string>> Chooser //优先于ChooseContinue
        {
            get
            {
                return _Chooser;
            }

            set
            {
                if (value == null && ChooseContinue == null)
                {
                    m_choose.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    m_choose.Visibility = System.Windows.Visibility.Visible;
                }
                _Chooser = value;
            }
        }

        public System.Action<System.Action<List<string>>> ChooseContinue //Chooser优先于ChooseContinue
        {
            get
            {
                return _ChooseContinue;
            }

            set
            {
                if (value == null && Chooser == null)
                {
                    m_choose.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    m_choose.Visibility = System.Windows.Visibility.Visible;
                }
                _ChooseContinue = value;
            }
        }


        private void m_choose_Click(object sender, RoutedEventArgs e)
        {
            if (Chooser != null)
            {
                var ls = Chooser();
                //弹出右键菜单
                var mContextMenu = new ContextMenu();
                foreach (var v in ls)
                {
                    MenuItem mi = new MenuItem();
                    mi.Header = v;

                    mi.Click += new RoutedEventHandler((obj, arg) =>
                    {
                        Val = v;
                    });
                    mContextMenu.Items.Add(mi);
                }

                mContextMenu.IsOpen = true;
            }
            else if (ChooseContinue != null)
            {
                ChooseContinue(ls =>
                {
                    var mContextMenu = new ContextMenu();
                    foreach (var v in ls)
                    {
                        MenuItem mi = new MenuItem();
                        mi.Header = v;

                        mi.Click += new RoutedEventHandler((obj, arg) =>
                        {
                            Val = v;
                        });
                        mContextMenu.Items.Add(mi);
                    }

                    mContextMenu.IsOpen = true;
                });
            }
        }

    }
}
