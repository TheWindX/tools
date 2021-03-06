﻿using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using vision_wpf;
using vision_wpf.views;

namespace ns_vision
{
    class RuntimeUtil : ns_utils.Singleton<RuntimeUtil>
    {
        #region logger
        WindowLogger mLogger = null;

        public RuntimeUtil()
        {
            mLogger = new WindowLogger();
        }
        public void error(params Object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var obj in objs)
            {
                sb.Append(obj.ToString());
            }
            mLogger.addInfo(sb.ToString());
        }

        public void log(params Object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var obj in objs)
            {
                sb.Append(obj.ToString());
            }
            mLogger.addInfo(sb.ToString());
        }

        public void ShowLogger()
        {
            mLogger.showAtCenter();
        }
        #endregion logger

        #region view
        public MainWindow getMainWindow()
        {
            return (App.Current as App).MainWindow as MainWindow;
        }
        
        public void popupContext(List<string> cmds, System.Action<string> handle)
        {
            var mContextMenu = new ContextMenu();
            for (int i = 0; i < cmds.Count; ++i)
            {
                MenuItem mi = new MenuItem();
                mi.Header = cmds[i];
                mi.Click += new RoutedEventHandler((obj, arg) =>
                {
                    handle(mi.Header.ToString());
                });
                mContextMenu.Items.Add(mi);
            }
            mContextMenu.IsOpen = true;
        }

        public void popStringEnter(System.Action<string> handle)
        {
            StringEnter.ShowOnTop(handle);
        }
        #endregion view


        #region runtime
        public CRuntime runtime
        {
            get
            {
                return (App.Current as App).runtime;
            }
        }
        #endregion


        #region test
        public void onTest(string testItem)
        {
            runtime.reset();
            MethodInfo methodInfo = RuntimeTest.Instance.GetType().GetMethod(testItem);
            methodInfo.Invoke(RuntimeTest.Instance, new object[] { });
        }
        #endregion





    }
}
