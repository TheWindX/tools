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
    public class MWorld
    {
        static List<EditorObject> mObjects = new List<EditorObject>();
        public static EditorObject createObject(EditorObject parent, string name)
        {
            EditorObject obj = new EditorObject();
            obj.parent = parent;
            obj.name = name;
            mObjects.Add(obj);
            return obj;
        }

        public static void removeObject(EditorObject obj)
        {
            obj.parent = null;
            mObjects.Remove(obj);
        }
    }
}
