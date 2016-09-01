using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [RequireCom(typeof(mapObject))]
    [CustomComponent(editable = false, isMain = true, name ="agent物体")]
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
        public double targetZ
        {
            get; set;
        }
    }
}
