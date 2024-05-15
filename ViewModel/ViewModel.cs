using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Controls;
using WpfApp2;
using System;

namespace OptimizationMethods
{
    public class ViewModel : INotifyPropertyChanged
    {

        NMconfig? newConfig = new NMconfig();
        List<Configuration> result = new List<Configuration>();
        string resultString = "";
        int resultIndex = -1;

        public NMconfig? NewConfig 
        { 
            get { return newConfig; }
            set
            {
                newConfig = value;
                OnPropertyChanged("NewConfig");
            }
        }

        public List<Configuration>? Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged("Result");
            }
        }

        public string? ResultString
        {
            get { return resultString; }
            set
            {
                if (value == null) resultString = string.Empty;
                else resultString = value;
                OnPropertyChanged("ResultString");
            }
        }

        public int ResultIndex
        {
            get { return resultIndex; }
            set
            {
                if (value < 0) return;
                if (value >= result.Count) return;
                resultIndex = value;
                if (result[value].simplex[0].coords.Length != 2)
                {
                    resultString = string.Empty;
                    return;
                }
                string result_string = "";
                for (int i = 0; i < result[value].simplex.Length; i++)
                {
                    if (i != 0) result_string += " ";
                    result_string += ((int)result[value].simplex[i].coords[0] * 10).ToString() + ",";
                    result_string += ((int)result[value].simplex[i].coords[1] * 10).ToString();
                }
                ResultString = result_string;
                OnPropertyChanged("ResultIndex");
            }
        }

        ICommand? left;
        public ICommand Left
        {
            get
            {
                if (left == null)
                {
                    left = new RelayCommand(
                        p => this.CanRun(),
                        p => { ResultIndex--; });
                }
                ResultIndex--;
                return left;
            }
        }
        ICommand? right;
        public ICommand Right
        {
            get
            {
                if (right == null)
                {
                    right = new RelayCommand(
                        p => this.CanRun(),
                        p => { ResultIndex++; });
                }
                ResultIndex--;
                return right;
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
            Result = nelderMeade.Run(NewConfig);
            ResultIndex = 0;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
