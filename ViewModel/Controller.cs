using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace OptimizationMethods.ViewModel
{
    public class Controller : INotifyPropertyChanged
    {
        public Controller() 
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
