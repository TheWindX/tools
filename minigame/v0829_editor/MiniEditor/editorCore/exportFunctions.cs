using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public static void save(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "root.xml";
            }

            //if (!File.Exists(path))
            //{
            //    MLogger.error("path of {0} is not exist", path);
            //    return;
            //}

            var str = EditorWorld.getRootEditorObject().toString();
            System.IO.File.WriteAllText(path, str);
        }

        public static void load(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "root.xml";
            }

            //if(!File.Exists(path) )
            //{
            //    MLogger.error("path of {0} is not exist", path);
            //    return;
            //}

            try
            {
                string text = System.IO.File.ReadAllText(path);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(text);
                var eo = MComponentExtender.editorObjectFromXML(null, doc.DocumentElement);
                EditorFuncs.getItemListPage().reflushEditorObject();
            }
            catch (Exception ex)
            {
                MLogger.error(ex.ToString());
            }
        }
    }
}
