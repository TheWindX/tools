﻿using System;
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

namespace ns_vision
{
    /// <summary>
    /// Interaction logic for UI_BaseTypeValue.xaml
    /// </summary>
    public partial class UI_ICON_UP : UserControl
    {
        public UI_ICON_UP()
        {
            InitializeComponent();
        }

        private void onMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                CBrowserModuleTreeManager.Instance.currentTreeBrowser.CDBackView();
            }
        }

        public void setSelect(bool b)
        {
            if (b)
            {
                var cb = (m_mask.Fill as SolidColorBrush);
                cb.Color = Color.FromArgb(40, cb.Color.R, cb.Color.G, cb.Color.B);
            }
            else
            {
                var cb = (m_mask.Fill as SolidColorBrush);
                cb.Color = Color.FromArgb(1, cb.Color.R, cb.Color.G, cb.Color.B);
            }
        }
    }
}
