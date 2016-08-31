using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    class MLogger
    {
        public static void info(string str, params object[] values)
        {
            if (values.Count() == 0)
                EditorFuncs.instance().getStat().addInfo(str);
            else
            {
                EditorFuncs.instance().getStat().addInfo(string.Format(str, values));
            }
        }

        public static void error(string str, params object[] values)
        {
            if (values.Count() == 0)
                EditorFuncs.instance().getStat().addError(str);
            else
            {
                EditorFuncs.instance().getStat().addError(string.Format(str, values));
            }
        }
    }
}
