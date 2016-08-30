using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MiniEditor.viewModel
{
    public class MapObject : MComponent
    {
        int x
        {
            get; set;
        }

        int y
        {
            get; set;
        }

        int radius
        {
            get; set;
        }

        Control getMapItem()
        {
            return null;
        }
    }
}
