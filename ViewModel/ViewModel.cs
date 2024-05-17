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

        public string ResultRepresentation
        {
            get
            {
                if (ResultIndex == -1) return "Not assigned";
                string output = "";
                for (int i = 0; i < result[ResultIndex].simplex.Length; i++)
                {
                    output += "[ ";
                    for (int j = 0; j < result[ResultIndex].simplex[i].coords.Length; j++)
                    {
                        output += result[ResultIndex].simplex[i].coords[j] + " ";
                    }
                    output += "] ";
                }
                return output;
            }
        }

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
                OnPropertyChanged("ResultRepresentation");
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
                ResultString = PlotView.StringGenerator(result[value]);
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

        ICommand? replot;
        public ICommand Replot
        {
            get
            {
                if (replot == null)
                {
                    replot = new RelayCommand(
                        p => this.CanRun(),
                        p => { ResultString = PlotView.StringGenerator(result[ResultIndex]);
                            OnPropertyChanged("ResultString");
                            OnPropertyChanged("ResultRepresentation");
                        });
                }
                return replot;
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
