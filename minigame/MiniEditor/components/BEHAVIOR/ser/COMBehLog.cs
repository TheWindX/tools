using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [CustomComponent(path = "BEHAVIOR", name = "调试")]
    class COMBehDebug : COMBeh
    {
        public string mContent = "debug here";
        public string content
        {
            get
            {
                return mContent;
            }
            set
            {
                mContent = value;
            }
        }
        public override bool behUpdate()
        {
            MLogger.info(content);
            return true;
        }
    }
}
