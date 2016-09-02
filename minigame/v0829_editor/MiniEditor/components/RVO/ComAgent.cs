using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [RequireCom(typeof(COMMapObject))]
    [CustomComponent(isMain = true, name ="agent物体", removable=false)]
    class ComAgent : MComponent
    {
        public double targetX
        {
            get; set;
        }

        public double targetY
        {
            get; set;
        }
    }
}
