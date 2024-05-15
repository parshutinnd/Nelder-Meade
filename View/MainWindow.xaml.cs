using System.Windows;
using OptimizationMethods;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Threading;
using System;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Binding bind;
        public MainWindow()
        {
            InitializeComponent();
            bind = new Binding();
        }
        private void DoublePreviewInput(object sender, TextCompositionEventArgs e)
        {
            string permitted = "0123456789+-.";
            e.Handled = !permitted.Contains(e.Text[0]);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
