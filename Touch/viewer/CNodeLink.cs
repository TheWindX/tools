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
            var b = new Binding();
            b.Source = this;
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
                var b = new Binding();
                b.Source = _leftNode;
                b.Path = new PropertyPath("Margin");
                b.Mode = BindingMode.TwoWay;
                b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(this, LeftPos, b);
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
                var b = new Binding();
                b.Source = _rightNode;
                b.Path = new PropertyPath("Margin");
                b.Mode = BindingMode.TwoWay;
                b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                BindingOperations.SetBinding(this, RightPos, b);
            }
        }

        public readonly static DependencyProperty LeftPos = DependencyProperty.Register("leftPos", typeof(Thickness), typeof(CNodeLink), new PropertyMetadata(propertyChanged));
        public readonly static DependencyProperty RightPos = DependencyProperty.Register("rightPos", typeof(Thickness), typeof(CNodeLink), new PropertyMetadata(propertyChanged));

        
        public static void propertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nl = (d as CNodeLink);
            if(nl.leftNode != null)
            {
                nl.begin = nl.leftNode.rightPos;
            }

            if (nl.rightNode != null)
            {
                nl.end = nl.rightNode.leftPos;
            }
        }

    }
}
