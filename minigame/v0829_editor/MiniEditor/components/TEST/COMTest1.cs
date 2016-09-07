using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [CustomComponent(path = "TEST", name = "test1", removable = false)]
    class COMTest1 : MComponent
    {
        public bool boolValue { get; set; }
        public int intValue { get; set; }
        public double doubleValue { get; set; }
        public string stringValue { get; set; }

        [Description]
        public string descripValue { get { return "description from test1"; } }

        public Action act
        {
            get
            {
                return () =>
                {
                    MLogger.info("log from COMTest1");
                };
            }
        }
    }
}
