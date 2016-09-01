using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiniEditor
{
    [CustomComponent(path = "global", name = "编辑器物体", removable = false)]
    class editorObject : MComponent
    {
        listItem mItem = null;
        int getLevel()
        {
            if (getObject().parent == null) return 0;
            else return getObject().parent.getComponent<editorObject>().getLevel() + 1;
        }

        public virtual listItem getMenuItem()
        {
            if(mItem == null)
            {   
                mItem = new listItem() { expand = true, isPick = false, level = getLevel(), name = getObject().name };
                mItem.editObject = getObject();
            }
            mItem.level = getLevel();
            return mItem;
        }

        public void reflushProp()
        {

        }

        public string name
        {
            get
            {
                return getMenuItem().name;
            }
            set
            {
                getObject().name = value;
                getMenuItem().name = value;
            }
        }

        static int idCount = 0;
        static int genID()
        {
            return idCount++;
        }

        int? mID = null;

        [EditorProperty]
        public string ID
        {
            get
            {
                if(mID == null)
                {
                    mID = genID();
                }
                return mID.Value.ToString();
            }
            set
            {

            }
        }
    }
}
