using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [CustomComponent(path = "TEST", name = "test2")]
    [RequireCom(typeof(COMTest1))]
    class COMTest2 : MComponent
    {
        public bool 布尔值 { get; set; }
        public int 整型值 {
            get
            {
                return getComponent<COMTest1>().intValue;
            }
            set
            {
                getComponent<COMTest1>().intValue = value;
            }
        }
        public double 双浮点 { get; set; }
        public string 字符串 { get; set; }

        [Description]
        public string descripValue { get { return "description from test2"; } }

        public Action act
        {
            get
            {
                return () =>
                {
                    MLogger.info("log from COMTest2");
                };
            }
        }
    }
}
