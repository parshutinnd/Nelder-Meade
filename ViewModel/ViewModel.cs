using OptimizationMethods.Model;
using OptimizationMethods.models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace OptimizationMethods.ViewModel
{
    public class ViewModel
    {
        public void Listerning()
        {

        }
        public ViewModel() 
        {
            NMconfig startConf = new NMconfig();
            NelderMeade algorithm = new NelderMeade(startConf);

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
