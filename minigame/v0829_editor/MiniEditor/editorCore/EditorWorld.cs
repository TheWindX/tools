using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    class EditorWorld
    {
        public static EditObject createObject(EditObject parent, string name)
        {
            var eo = MWorld.createObject(parent, name);
            eo.addComponent<editorUICOM>();
            return eo;
        }

        public static void removeObject(EditObject obj)
        {
            MWorld.removeObject(obj);
        }

    }
}
