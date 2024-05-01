using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace OptimizationMethods
{
    public class ViewModel : INotifyPropertyChanged
    {

        NMconfig? newConfig = new NMconfig();

        public NMconfig? NewConfig 
        { 
            get { return newConfig; }
            set
            {
                newConfig = value;
                OnPropertyChanged("NewConfig");
            }
        }

        private ICommand? _start;
        public ICommand StartCommand
        {
            get
            {
                if (_start == null)
                {
                    _start = new RelayCommand(
                        p => this.CanRun(),
                        p => this.RunNelderMeade());
                }
                return _start;
            }
        }

        public bool CanRun()
        {
            return true;
        }

        public void RunNelderMeade()
        {
            NelderMeade nelderMeade = new NelderMeade(NewConfig);
            var results = nelderMeade.Run(NewConfig);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
