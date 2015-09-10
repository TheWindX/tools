using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_vision
{
    public partial class RuntimeTest : Singleton<RuntimeTest>
    {
        public void test1()
        {
            var bs = (App.Current.MainWindow as MainWindow).m_mainBrowser.runtimeBrowser;
            bs.mkFold("fa");
            bs.mkFold("faa");
            bs.mkFold("faaa");
            bs.cdback();
            bs.cdback();
            bs.mkModule("ma");
            bs.mkModule("maa");
            bs.mkModule("maaa");
            bs.cdback();
            bs.mkInt(100);
            bs.resetTop();
            var mt = bs.currentSpace.getComponent<CModuleItem>();
            mt.print(0);
        }
        
        #region test2
        public void test2()
        {
            var bsView = (App.Current.MainWindow as MainWindow).m_mainBrowser;
            var bs = bsView.runtimeBrowser;
            bs.mkFold("fa");
            bs.mkFold("faa");
            bs.mkFold("faaa");
            bs.cdback();
            bs.cdback();
            bs.mkModule("ma");
            bs.mkModule("maa");
            bs.mkModule("maaa");
            bs.cdback();
            bs.mkInt(100);
            bs.resetTop();
            bsView.SetCurrentSpace(bs.currentSpace);
        }
        #endregion test2
    }
}
