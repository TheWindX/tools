using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEditor
{
    [RequireCom(typeof(ComAgent))]
    [CustomComponent(editable = false, name = "agent物体编辑")]
    class ComEditagent : MComponent
    {
        [Description]
        public string desc
        {
            get
            {
                return "这是一个很长的描述描述描述描述描述描述描述描述描述描述描述描述描述描述描述描述描述描述描述";
            }
        }

        [CustomInspector]
        public void inspector()
        {

        }
    }


}
