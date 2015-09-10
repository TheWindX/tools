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
    public partial class CTypeConstructorPara : Component
    {
        public override void inherit()
        {
            addComponent<CNamed>();
        }

        Ctype _type = null;
        public Ctype type_
        {
            get
            {
                return _type;
            }
                
            set
            {
                _type = value;//涉及改动view
            }
        }

        public CTypeConstructor parent = null;
    }
}