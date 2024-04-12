using System.Windows;
using OptimizationMethods.ViewModel;

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

            Controller AlgorithmController = new Controller();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
