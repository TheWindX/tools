using MiniEditor;
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

namespace MiniEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditorFuncs.instance().openHELP();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            MDriver.init();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            MDriver.update();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MDriver.exit();
        }
    }
}
