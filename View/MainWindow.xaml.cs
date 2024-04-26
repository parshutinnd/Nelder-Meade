using System.Windows;
using OptimizationMethods.ViewModel;
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
        ViewModel AlgorithmController;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel AlgorithmViewModel = new ViewModel();
            new Thread(AlgorithmViewModel.Listerning).Start();
            AlgorithmViewModel.Listerning();
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

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
