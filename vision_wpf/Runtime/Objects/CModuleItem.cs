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
    public partial class CModuleItem : Component
    {
        public override void inherit()
        {
            addComponent<CNamed>();
        }

        public CModuleTree parent;

        public System.Action<int> _HandlePrint;

        public override void init()
        {
            _HandlePrint = space =>
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < space; ++i)
                    sb.Append(" ");
                sb.Append(this.getComponent<CNamed>().name);
                RuntimeUtil.Instance.log(sb.ToString());
            };

            //override of CModuleItem._HandleDrawIcon
            _HandleDrawIcon = () =>
            {
                mIcon.Text = getComponent<CNamed>().name;
                return mIcon;
            };
            //override of CModuleItem._HandleSelect
            _HandleSelect = b =>
            {
                if (b)
                    mUI.Background = new SolidColorBrush(Colors.AliceBlue);
                else
                    mUI.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            };
        }
        TextBlock mIcon = new TextBlock();

        public void print(int space)
        {
            _HandlePrint(space);
        }

        public System.Func<FrameworkElement> _HandleDrawIcon;
        TextBlock mUI = new TextBlock();
        public FrameworkElement drawIcon()
        {
            return _HandleDrawIcon();
        }

        public System.Action<bool> _HandleSelect;
        public void select(bool s)
        {
            _HandleSelect(s);
        }
    }


}