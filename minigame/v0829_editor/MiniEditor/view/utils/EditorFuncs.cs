﻿
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
        static WindowsF1 mHelpWindow = null;
        public static void openHELP()
        {
            init();
            if (mHelpWindow == null)
                mHelpWindow = new WindowsF1();
            mHelpWindow.Owner = mMainWindow;

            mHelpWindow.Show();
            mHelpWindow.Left = mMainWindow.Left + (mMainWindow.Width - mHelpWindow.ActualWidth) / 2;
            mHelpWindow.Top = mMainWindow.Top + (mMainWindow.Height - mHelpWindow.ActualHeight) / 2;
            mHelpWindow.Activate();
        }
        
        public static StatusPage getStatPage()
        {
            init();
            return mMainWindow.mStatusPage;
        }

        private static MainWindow mMainWindow = null;
        private static void init()
        {
            if (mMainWindow == null)
            {
                mMainWindow = (App.Current.MainWindow as MainWindow);
            }
            return;
        }

        public static ComponentPanelList getComponentPage()
        {
            init();
            return mMainWindow.mListComponent;
        }

        public static editorMap getMapPage()
        {
            init();
            return mMainWindow.mMapPage;
        }

        public static itemList getItemListPage()
        {
            init();
            return mMainWindow.mListObjects;
        }

        public static Point getMousePositionInMap()
        {
            Point p = Mouse.GetPosition(getMapPage().mCanvas);
            return p;
        }

        internal static void doKeyUp(Key k)
        {
            if(evtKey != null)
            {
                evtKey(k);
            }
        }

        public static event System.Action<Key> evtKey = null;

        public static bool isLeftControlPressed()
        {
            return Keyboard.IsKeyDown(Key.LeftCtrl);
        }

        public static bool isLeftShiftPressed()
        {
            return Keyboard.IsKeyDown(Key.LeftShift);
        }

        public static bool isRightControlPressed()
        {
            return Keyboard.IsKeyDown(Key.RightCtrl);
        }

        public static bool isRightShiftPressed()
        {
            return Keyboard.IsKeyDown(Key.RightShift);
        }

        public static EditorObject getRootEditorObject()
        {
            return EditorWorld.getRootEditorObject();
        }

        public static EditorObject getCurrentEditorObject()
        {
            return getItemListPage().getCurrentObj();
        }
    }
}
