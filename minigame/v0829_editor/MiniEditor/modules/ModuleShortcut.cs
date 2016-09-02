using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [ModuleInstance(0)]
    class ModuleShortcut : MModule
    {
        public void onExit()
        {
            EditorFuncs.evtKey -= EditorFuncs_evtKey;
        }

        public void onInit()
        {
            EditorFuncs.evtKey += EditorFuncs_evtKey;
        }

        public void onUpdate()
        {

        }

        int icount = 0;
        private void EditorFuncs_evtKey(System.Windows.Input.Key k)
        {
            if(k == System.Windows.Input.Key.Insert)
            {
                var obj = EditorWorld.createObject(EditorFuncs.getCurrentEditorObject(), "magic"+icount++.ToString());
                EditorFuncs.getItemListPage().insertEditorItem(obj);
            }
        }


    }
}
