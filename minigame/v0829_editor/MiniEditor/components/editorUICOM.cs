﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiniEditor
{
    [ComponentCustom(path = "global", name ="editor", removable = false)]
    class editorCOM : MComponent
    {
        listItem mItem = null;
        int getLevel()
        {
            if (getObject().parent == null) return 0;
            else return getObject().parent.getComponent<editorCOM>().getLevel() + 1;
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
    }
}
