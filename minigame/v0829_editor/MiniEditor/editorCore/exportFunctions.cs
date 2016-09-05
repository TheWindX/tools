using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    public class exports
    {
        public static void print(string text)
        {
            MLogger.info(text);
        }

        public static void help()
        {
            EditorFuncs.openHELP();
        }

        public static void printCurrent()
        {
            EditorWorld.getRootEditorObject().printOBJ();
        }
    }
}
