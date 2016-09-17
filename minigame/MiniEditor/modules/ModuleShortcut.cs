/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
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

        private static int idCount = 1;
        private void EditorFuncs_evtKey(System.Windows.Input.Key k)
        {
            if(k == System.Windows.Input.Key.Insert)
            {
                var obj = EditorWorld.createObject(EditorFuncs.getCurrentEditorObject(), "item_"+ idCount++);
                EditorFuncs.getItemListPage().insertEditorObject(obj);
            }
            else if(k == System.Windows.Input.Key.F1)
            {
                EditorFuncs.openHELP();
            }
        }
    }
}
