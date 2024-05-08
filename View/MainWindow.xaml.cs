using System.Windows;
using OptimizationMethods;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Threading;
using System;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }
        private void DoublePreviewInput(object sender, TextCompositionEventArgs e)
        {
            string permitted = "0123456789+-.";
            e.Handled = !permitted.Contains(e.Text[0]);
        }
    }
}
