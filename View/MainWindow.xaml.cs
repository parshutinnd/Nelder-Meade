using System.Windows;
using OptimizationMethods;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Threading;

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
        private void IntInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[-+]?[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void FloatInput(object sender, TextCompositionEventArgs e)
        {
            /* check for float */
        }
    }
}
