using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [MiniEditor.ModuleInstance(1)]
    class ModuleTestListItem : MModule
    {
        public void onExit()
        {
            
        }

        public void onInit()
        {
            var p = EditorFuncs.instance().getItemListPage();
            MTimer.get().setTimeout(t =>
            {
                var root = EditorWorld.createObject(null, "root");
                var a1 = EditorWorld.createObject(root, "a1");
                var a2 = EditorWorld.createObject(a1, "a2");
                var a3 = EditorWorld.createObject(a2, "a3");
                
                p.addEditorItem(root);
            }, 1500);

            MTimer.get().setTimeout(t =>
            {
                var a4 = EditorWorld.createObject(null, "a4");
                p.insertEditorItem(a4);
            }, 4000);
        }

        public void onUpdate()
        {
            
        }
    }
}
