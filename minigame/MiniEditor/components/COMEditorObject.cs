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
using System.Windows.Controls;

namespace MiniEditor
{
    [CustomComponent(path = "CORE", name = "编辑器物体", removable = false)]
    class COMEditorObject : MComponent
    {
        listItem mItem = null;//TODO, 放入一个隐藏层
        int getLevel()
        {
            if (getEditorObject().parent == null) return 0;
            else return getEditorObject().parent.getComponent<COMEditorObject>().getLevel() + 1;
        }

        public virtual listItem getMenuItem()
        {
            if (mItem == null)
            {
                mItem = new listItem() { expand = true, isPick = false, level = getLevel(), name = getEditorObject().name };
                mItem.editObject = getEditorObject();
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
                getEditorObject().name = value;
                getMenuItem().name = value;
            }
        }

        //static int idCount = 0;
        static string genID()
        {
            return MRandom.randString(12);
        }

        string mID = null;

        //[PropertyDisable]
        public string ID
        {
            get
            {
                if (mID == null)
                {
                    mID = genID();
                }
                return mID;
            }
            set
            {

            }
        }
    }
}
