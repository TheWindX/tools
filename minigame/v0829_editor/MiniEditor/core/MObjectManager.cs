using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    public class MWorld
    {
        static List<EditObject> mObjects = new List<EditObject>();
        public static EditObject createObject(EditObject parent, string name)
        {
            EditObject obj = new EditObject();
            obj.parent = parent;
            obj.name = name;
            mObjects.Add(obj);
            return obj;
        }

        public static void removeObject(EditObject obj)
        {
            obj.parent = null;
            mObjects.Remove(obj);
        }
    }
}
