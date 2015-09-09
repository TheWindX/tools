using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vision_wpf.views;

namespace ns_vision
{
    class RuntimeUtil : Singleton<RuntimeUtil>
    {
        WindowLogger mLogger = null;

        public RuntimeUtil()
        {
            mLogger = new WindowLogger();
        }
        public void log(params Object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var obj in objs)
            {   
                sb.Append(obj.ToString());
            }
            mLogger.addInfo(sb.ToString());
        }

        public void error(params Object[] objs)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var obj in objs)
            {
                sb.Append(obj.ToString());
            }
            mLogger.addInfo(sb.ToString());
        }

        public void ShowLogger()
        {
            mLogger.showAtCenter();
        }
    }
}
