using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
    /// intField.xaml 的交互逻辑
    /// </summary>
    public partial class boolField : UserControl
    {
        public boolField()
        {
            InitializeComponent();
            m_value.IsChecked = false;
            m_value.Checked += hValueChange;
            m_value.Unchecked += hValueChange;
            this.LostFocus += hLostFocus;
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

        public bool Val
        {
            get
            {
                return m_value.IsChecked.Value;
            }
            set
            {
                m_value.IsChecked = value;
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
    }
}
