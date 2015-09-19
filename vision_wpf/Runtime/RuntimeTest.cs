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
            var bs = (App.Current.MainWindow as MainWindow).m_leftBrowser.runtimeBrowser;
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
            var leftView = CBrowserModuleTreeManager.Instance.currentTreeBrowser;
            var righView = CBrowserModuleTreeManager.Instance.otherTreeBrowser;
            var bs = leftView.runtimeBrowser;
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
            leftView.updateView();
            righView.updateView();
        }
        #endregion test2
    }
}
