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
        static List<MObject> mObjects = new List<MObject>();
        public static MObject createObject(MObject parent, string name)
        {
            MObject obj = new MObject();
            obj.parent = parent;
            obj.name = name;
            mObjects.Insert(0, obj);
            return obj;
        }

        public static void removeObject(MObject obj)
        {
            obj.parent = null;
            mObjects.Remove(obj);
        }
    }
}
