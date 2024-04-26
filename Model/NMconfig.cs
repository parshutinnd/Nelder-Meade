using FunctionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OptimizationMethods.Domain;

namespace OptimizationMethods.Model
{
    public class NMconfig
    {
        public int n; //размерность
        public Expression exp; //функция
        public double alpha; //коэффициент отражения
        public double beta = 2;  //коэффициент растяжения
        public double gamma = 0.5; //коэффициент сжатия
        public double l = 1; //начальное отклонение
        public double epsilon; //эпсилон
        bool isReady = false;
        public Point[] startSimplex;

        public NMconfig()
        {
            this.n = 0;
            this.exp = null;
            this.epsilon = 0;
            this.alpha = 1;
            this.beta = 1;
            this.gamma = 1;
            this.isReady = false;
        }
        public NMconfig(int n, Expression exp, double epsilon, double alpha, double beta, double gamma)
        {
            this.n = n;
            this.exp = exp;
            this.epsilon = epsilon;
            this.alpha = alpha;
            this.beta = beta;
            this.gamma = gamma;
            this.isReady = IsReadyToStart();
        }
        public void GetByPoint(Point start) //По начальной точке
        {
            this.startSimplex = GenerateSimplex(start);
        }

        public void GetBySimplex(Point[] startSimplex) //По начальному симплексу
        {
            this.startSimplex = Point.Clone(startSimplex);
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
                if ((startSimplex[i] != null) || (n != startSimplex[i].dimension))
                    return false;
            //условие 3 (Корректные параметры алгоритма)
            if ((alpha <= 0) || (beta <= 1) || (gamma <= 0) || (gamma >= 1) || (epsilon <= 0)) return false;
            //условие 4 (Начальный симплекс линейно независим)
            if (!IsConvexHull()) isReady = false;
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
    }
}
