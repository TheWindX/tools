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
    public partial class CTypeConstructor : Component
    {
        public override void inherit()
        {
            addComponent<Ctype>();
        }

        public ListAdvance<CTypeConstructorPara> paras = new ListAdvance<CTypeConstructorPara>();

        public FrameworkElement drawUI()
        {
            return null;
        }

        public void createTypePara(string name)
        {
            var tp = new CTypeConstructorPara();
            tp.getComponent<CNamed>().name = name;
            tp.parent = this;
            paras.Add(tp);
        }
    }
}