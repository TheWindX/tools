using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using vision_wpf;
using vision_wpf.views;

namespace ns_vision
{
    class RuntimeUtil : Singleton<RuntimeUtil>
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

        public void setMainPanel(FrameworkElement ui)
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
                mRuntime.cdback();
                setMainPanel(mRuntime.currentSpace.drawUI());
            }
        }

        
    }
}
