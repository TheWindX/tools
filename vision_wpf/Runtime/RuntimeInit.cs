using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ns_vision
{
    class RuntimeInit : Singleton<RuntimeInit>
    {
        public void init()
        {
            CBrowserModuleTreeManager.Instance.init();
        }
    }
}
