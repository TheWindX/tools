/*
 * author: xiaofeng.li
 * mail: 453588006@qq.com
 * desc: 
 * */
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
                EditorFuncs.instance().getStatPage().addInfo(str);
            else
            {
                EditorFuncs.instance().getStatPage().addInfo(string.Format(str, values));
            }
        }

        public static void error(string str, params object[] values)
        {
            if (values.Count() == 0)
                EditorFuncs.instance().getStatPage().addError(str);
            else
            {
                EditorFuncs.instance().getStatPage().addError(string.Format(str, values));
            }
        }
    }
}
