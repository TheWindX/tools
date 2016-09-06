using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    public class exports
    {
        //public static void print(string text)
        //{
        //    MLogger.info(text);
        //}

        public static void help()
        {
            EditorFuncs.openHELP();
        }

        public static void print()
        {
            EditorWorld.getRootEditorObject().printOBJ();
        }

        public static void clear()
        {
            EditorFuncs.getItemListPage().removeEditorObjectChildren(EditorWorld.getRootEditorObject());
        }

        public static void clearCurrent()
        {
            var obj = EditorWorld.getCurrentObj();
            EditorFuncs.getItemListPage().removeEditorObjectChildren(obj);
        }
    }
}
