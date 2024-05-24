using FunctionParser;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OptimizationMethods
{
    public class NMconfig : INotifyPropertyChanged
    {
        public int n; //размерность
        public string[] ids; // examample: X1 X2 X3
        public string function; //строковое представление
        public Expression exp; //функция
        public double alpha; //коэффициент отражения
        public double beta;  //коэффициент растяжения
        public double gamma; //коэффициент сжатия
        public double l; //начальное отклонение
        public double epsilon; //эпсилон
        public bool isReady;
        public string startPoint; //строковое представление
        public Point[] startSimplex;

        public string Function //функция
        {
            get { return function; }
            set
            {
                function = value;
                exp = new Expression(function,ids,null) ;
                n = 0;
                while (function.Contains($"X{n + 1}"))
                {
                    n++;
                }
                ids = new string[n];
                for (int i = 0; i < n; i++)
                {
                    ids[i] = $"X{i + 1}";
                }
                OnPropertyChanged("function");
            }
        }
        public double Alpha //коэффициент отражения
        {
            get { return alpha; }
            set
            {
                alpha = value;
                OnPropertyChanged("alpha");
            }
        }
        public double Beta  //коэффициент растяжения
        {
            get { return beta; }
            set
            {
                beta = value;
                OnPropertyChanged("beta");
            }
        }
        public double Gamma //коэффициент сжатия
        {
            get { return gamma; }
            set
            {
                gamma = value;
                OnPropertyChanged("gamma");
            }
        }
        public double L //начальное отклонение
        {
            get { return l; }
            set
            {
                l = value;
                OnPropertyChanged("l");
            }
        }
        public double Epsilon //эпсилон
        {
            get { return epsilon; }
            set
            {
                epsilon = value;
                OnPropertyChanged("epsilon");
            }
        }

        public string Start
        {
            get { return startPoint; }
            set 
            {
                startPoint = value;
                startSimplex = GetByPoint(new(startPoint));
                OnPropertyChanged("startPoint");
            }
        }

        public NMconfig()
        { 
            this.ids = new string[] { "X1", "X2" };
            this.n = ids.Length;
            this.function = "X1^2+X2^2";
            this.exp = new Expression(function,ids,null);
            this.epsilon = 0.01;
            this.alpha = 1;
            this.beta = 2;
            this.gamma = 0.5;
            this.l = 1;
            this.startPoint = "5; 5";
            this.startSimplex = GetByPoint(new(startPoint)); 
        }

        public Point[] GetByPoint(Point start) //По начальной точке
        {
            return GenerateSimplex(start);
        }

        Point[] GenerateSimplex(Point startPoint)
        {
            int n = startPoint.dimension;
            Point[] startSimplex = new Point[startPoint.dimension + 1];
            double[] startPointCoords = startPoint.coords;
            startSimplex[0] = new Point(startPointCoords);
            for (int i = 0; i < n; i++)
            {
                startPointCoords[i] += l;
                startSimplex[i + 1] = new Point(startPointCoords);
                startPointCoords[i] -= l;
            }
            return startSimplex;
        }

        public bool IsReadyToStart() //проверка к старту алгоритма
        {
            //условие 1 (Правильный размер симплекса)
            if (n + 1 != startSimplex.Length) return false;
            //условие 2 (Размерности точек совпадают)
            for (int i = 0; i < startSimplex.Length; i++)
                if ((startSimplex[i] == null) || (n != startSimplex[i].dimension))
                    return false;
            //условие 3 (Корректные параметры алгоритма)
            if ((alpha <= 0) || (beta <= 1) || (gamma <= 0) || (gamma >= 1) || (epsilon <= 0)) return false;
            //условие 4 (Начальный симплекс линейно независим)
            if (!IsConvexHull()) return false;
            return true;
        }

        public bool IsConvexHull() //проверка линейной независимости
        {
            Point[] vectors = new Point[n];
            for (int i = 0; i < n; i++)
                vectors[i] = startSimplex[i + 1] - startSimplex[0];
            for (int i = 0; i < n; i++)
                for (int j = i + 1; j < n; j++)
                    if (!Point.LinearIndependence(vectors[i], vectors[j])) return false;
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
