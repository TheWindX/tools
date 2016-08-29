using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_MiniGame
{
    public interface MModule
    {
        void onInit();
        void onUpdate();
        void onExit();
    }
}
