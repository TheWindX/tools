using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [CustomComponent(path = "DEMO/RVO", name = "agent 管理器", removable = false)]
    class COMAgentManager : MComponent
    {
        public System.Action addAgent
        {
            get
            {
                return () =>
                {
                    var obj = EditorWorld.createObject(getObject(), "agent");
                    obj.addComponent<COMEditagent>();
                    EditorFuncs.getItemListPage().insertEditorObject(obj);
                };
            }
        }
    }
}
