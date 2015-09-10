using ns_vision.ns_utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ns_vision
{
    public partial class CRuntimeObj : Component
    {
        public CRuntime runtime;
        public override void inherit()
        {
            //addComponent<CNamed>();
        }
    }
}