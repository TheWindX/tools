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
    [RequireCom(typeof(COMMapObjectPicker))]
    [CustomComponent(path ="DEMO/RVO", name ="agent物体", removable=false)]
    class COMAgent : MComponent
    {
        public double targetX
        {
            get; set;
        }

        public double targetY
        {
            get; set;
        }

        public int agentID
        {
            get; set;
        }
    }
}
