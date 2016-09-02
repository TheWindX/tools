
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        //public void openStat()
        //{
        //    var statWindow = getStatWindow();
        //    mStatWindow.Owner = mMainWindow;
        //    statWindow.Show();
        //    statWindow.Left = mMainWindow.Left + (mMainWindow.Width - statWindow.ActualWidth) / 2;
        //    statWindow.Top = mMainWindow.Top + (mMainWindow.Height - statWindow.ActualHeight) / 2;
        //    statWindow.Activate();
        //}
        public StatusPage getStatPage()
        {
            init();
            return mMainWindow.mStatusPage;
        }

        private MainWindow mMainWindow = null;
        private void init()
        {
            if (mMainWindow == null)
            {
                mMainWindow = (App.Current.MainWindow as MainWindow);
            }
            return;
        }

        public ComponentPanelList getComponentPage()
        {
            if(mMainWindow != null)
            {
                init();
            }
            return mMainWindow.mListComponent;
        }

        public editorMap getMapPage()
        {
            if (mMainWindow != null)
            {
                init();
            }
            return mMainWindow.mMapPage;
        }

        public itemList getItemListPage()
        {
            if (mMainWindow != null)
            {
                init();
            }
            return mMainWindow.mListObjects;
        }

        public Point getMousePositionInMap()
        {
            Point p = Mouse.GetPosition(getMapPage().mCanvas);
            return p;
        }
    }
}
