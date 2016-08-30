﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace MiniEditor
{
    //编辑器控制器
    class EditorFuncs
    {
        private static EditorFuncs ins = null;
        public static EditorFuncs instance()
        {
            if (ins == null)
            {
                ins = new EditorFuncs();
                ins.init();
            }
            return ins;
        }

        WindowsF1 mHelpWindow = null;
        public void openHELP()
        {
            if (mHelpWindow == null)
                mHelpWindow = new WindowsF1();
            mHelpWindow.Owner = mMainWindow;

            mHelpWindow.Show();
            mHelpWindow.Left = mMainWindow.Left + (mMainWindow.Width - mHelpWindow.ActualWidth) / 2;
            mHelpWindow.Top = mMainWindow.Top + (mMainWindow.Height - mHelpWindow.ActualHeight) / 2;
            mHelpWindow.Activate();
        }

        public void openStat()
        {
            var statWindow = getStatWindow();
            mStatWindow.Owner = mMainWindow;
            statWindow.Show();
            statWindow.Left = mMainWindow.Left + (mMainWindow.Width - statWindow.ActualWidth) / 2;
            statWindow.Top = mMainWindow.Top + (mMainWindow.Height - statWindow.ActualHeight) / 2;
            statWindow.Activate();
        }

        WindowStaus mStatWindow = null;
        public WindowStaus getStatWindow()
        {
            if (mStatWindow == null)
            {
                mStatWindow = new WindowStaus();
                mStatWindow.evtClear += () => {
                    mMainWindow.m_statbar.Content = "";
                    mMainWindow.m_statbar.Foreground = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
                };
            }

            return mStatWindow;
        }

        private MainWindow mMainWindow = null;
        private void init()
        {
            mMainWindow = (App.Current.MainWindow as MainWindow);
            return;
        }

        public StackPanel getPropWindow()
        {
            if(mMainWindow != null)
            {
                init();
            }
            return mMainWindow.m_propertys;
        }

        public itemList getItemListWindow()
        {
            if (mMainWindow != null)
            {
                init();
            }
            return mMainWindow.mListView;
        }
    }
}