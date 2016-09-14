using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [CustomComponent(path = "DEMO/RVO", name = "obstacle 管理器", removable = false)]
    class COMObstacleManager : MComponent
    {
        public System.Action addObstacle
        {
            get
            {
                return () =>
                {
                    var obj = EditorWorld.createObject(getEditorObject(), "obstacle");
                    obj.addComponent<COMRVOObstacle>();
                    EditorFuncs.getItemListPage().insertEditorObject(obj);
                };
            }
        }
    }
}
