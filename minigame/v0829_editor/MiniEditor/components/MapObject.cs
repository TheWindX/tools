﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiniEditor
{
    [CustomComponent(path = "global", name ="地图物体")]
    public class mapObject : MComponent
    {
        public int x
        {
            get; set;
        }

        public int y
        {
            get; set;
        }

        public int radius
        {
            get; set;
        }
        public Control getMapItem()
        {
            return null;
        }
    }
}
