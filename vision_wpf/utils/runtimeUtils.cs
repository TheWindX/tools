using ns_vision.ns_utils;
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
        WindowLogger mLogger = null;

        public RuntimeUtil()
        {
            mLogger = new WindowLogger();
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

        public void error(params Object[] objs)
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

        private void setMainPanel(FrameworkElement ui)
        {
            var win = App.Current.MainWindow as MainWindow;
            win.m_panel.Children.Clear();
            win.m_panel.Children.Add(ui);
        }

        CRuntime mRuntime = new CRuntime();
        public void onTest(string testItem)
        {
            mRuntime.reset();
            MethodInfo methodInfo = mRuntime.GetType().GetMethod(testItem);
            methodInfo.Invoke(mRuntime, new object[] { });
        }

        public void onKeyUp(System.Windows.Input.Key kc)
        {
            if(kc == System.Windows.Input.Key.Back)
            {
                CDBackView();
            }
        }

        public void CDBackView()
        {
            mRuntime.cdback();
            SetCurrentSpace(mRuntime.currentSpace);
        }

        private void SetCurrentSpaceView(CModuleTree tree)
        {   
            setMainPanel(mRuntime.currentSpace.drawUI());
        }

        public void SetCurrentSpace(CModuleTree tree)
        {
            tree.getComponent<CRuntimeObj>().runtime.currentSpace = tree;
            tree.getComponent<CRuntimeObj>().runtime.selected = null;
            SetCurrentSpaceView(tree);
            var win = App.Current.MainWindow as MainWindow;
            win.m_adress.runtimeObject = tree.getComponent<CModuleItem>();
        }

        public void SetSelect(CModuleItem mi)
        {
            if (mi != null)
            {
                var tree = mi.parent;
                if (tree == null) return;
                mi.getComponent<CRuntimeObj>().runtime.currentSpace = tree;
                SetCurrentSpace(tree);

                foreach (var c in tree.children)
                {
                    c.select(false);
                }
                mi.select(true);
            }
        }

        public void popupContext(List<string> cmds, System.Action<string> handle)
        {
            var mContextMenu = new ContextMenu();
            for(int i = 0; i<cmds.Count; ++i)
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

        public void updateView()
        {
            setMainPanel(mRuntime.currentSpace.drawUI());
        }
        
    }
}
