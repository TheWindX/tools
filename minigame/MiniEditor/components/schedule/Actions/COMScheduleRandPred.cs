using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [CustomComponent(path = "SCHEDULE/ATOM", name = "随机返回")]
    class COMScheduleRandPred : COMSchedule
    {
        public override bool scheduleExit()
        {
            base.scheduleExit();
            return MRandom.randBool();
        }
    }
}
