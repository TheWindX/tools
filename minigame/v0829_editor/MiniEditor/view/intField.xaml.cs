﻿using System;
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
    public partial class intField : UserControl
    {
        public intField()
        {
            InitializeComponent();
            
            m_value.TextChanged += hValueChange;
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

        public int Val
        {
            get
            {
                int val = 0;
                int.TryParse(m_value.Text, out val);
                return val;
            }
            set
            {
                m_value.Text = value.ToString();
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

        internal static bool sIsTextAllowed(string text)
        {
            if (text.Length > 6) return false;
            Regex regex = new Regex(@"^-?[0-9]*$"); //regex that matches disallowed text
            var r = regex.IsMatch(text);
            return r;
        }

        // Use the DataObject.Pasting Handler 
        internal static void sTextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!sIsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void textInput(object sender, TextCompositionEventArgs e)
        {
            if (intField.sIsTextAllowed(m_value.Text) && intField.sIsTextAllowed(e.Text))
                e.Handled = false;
            else
                e.Handled = true;
        }

        Point mPoint = new Point();
        private void m_label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mPoint = e.GetPosition(sender as TextBlock);
            m_value.TextChanged -= hValueChange;
            m_label.CaptureMouse();
        }

        private void m_label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (m_label.IsMouseCaptured)
            {
                m_value.TextChanged += hValueChange;
                m_label.ReleaseMouseCapture();
                if (evtValueChanged != null) evtValueChanged();
            }
        }

        static int triggerCount = 0;
        const int triggerCountMax = 5;
        private void m_label_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_label.IsMouseCaptured)
            {
                var nowPos = e.GetPosition(sender as TextBlock);
                var delta = (nowPos - mPoint);
                mPoint = nowPos;
                Val += (int)(delta.X);
                triggerCount++;
                if(triggerCount > triggerCountMax)
                {
                    if (evtValueChanged != null) evtValueChanged();
                }
            }
        }


    }
}
