using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiniEditor.viewModel
{
    [ComponentCustom(path = "global", name ="坐标")]
    public class MapObject : MComponent
    {
        public int x
        {
            get; set;
        }

        public int y
        {
            get; set;
        }

        public int radius
        {
            get; set;
        }
        public Control getMapItem()
        {
            return null;
        }
    }
}
