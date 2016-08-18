using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Touch
{
    /// <summary>
    /// Interaction logic for Line.xaml
    /// </summary>
    public partial class CNodeLink : CLink
    {
        public CNodeLink():base()
        {
            MouseLeftButtonDown += CNodeLink_MouseLeftButtonDown;
            KeyDown += CNodeLink_KeyDown;
        }

        private void CNodeLink_KeyDown(object sender, KeyEventArgs e)
        {

        }

        CNode _leftNode;
        public CNode leftNode
        {
            get
            {
                return _leftNode;
            }
            set
            {
                _leftNode = value;
                begin = _leftNode.rightPos;
            }
        }

        CNode _rightNode;
        public CNode rightNode
        {
            get
            {
                return _rightNode;
            }
            set
            {
                _rightNode = value;
                end = _rightNode.leftPos;
            }
        }

        public readonly static DependencyProperty LeftPos = DependencyProperty.Register("leftPos", typeof(Thickness), typeof(CNodeLink));
        public readonly static DependencyProperty RightPos = DependencyProperty.Register("rightPos", typeof(Thickness), typeof(CNodeLink));

        public void updateView()
        {
            if (leftNode != null)
            {
                begin = leftNode.rightPos;
            }

            if (rightNode != null)
            {
                end = rightNode.leftPos;
            }
        }

        Color choosed = Color.FromArgb(122, 0, 0, 20);
        Color unChoosed = Color.FromArgb(255, 0, 0, 0);
        public bool selected
        {
            set
            {
                if (value)
                {
                    m_path.Stroke = new SolidColorBrush(choosed);
                    this.Focusable = true;
                    Keyboard.Focus(this);
                }   
                else
                {
                    m_path.Stroke = new SolidColorBrush(unChoosed);
                }
                    
            }
        }

        private void CNodeLink_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(evtSelect != null)
            {
                evtSelect();
            }
        }

        public System.Action evtSelect;
    }
}
