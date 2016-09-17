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
    [CustomComponent(path = "SCHEDULE/ATOM", name = "打印")]
    class COMScheduleDebug : COMSchedule
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
        public override bool scheduleUpdate()
        {
            MLogger.info(content);
            return true;
        }
    }
}
