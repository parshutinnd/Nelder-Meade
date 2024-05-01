using System;
using FunctionParser;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethods
{
    enum Actions
    {
        Reflection,       //отражение
        Stretching,       //растяжение
        Compression,      //сжатие
        GlobalCompression //глобальное сжатие
    }
    class Configuration //сюда записываем промежуточные результаты работы
    {
        public readonly Point[] simplex; //симплекс
        public readonly Actions action;  //следущее действие
        public readonly Point centrePoint; //вспомогательные точки
        public readonly Point reflectPoint;
        public readonly Point stretchPoint;
        public readonly Point сompressPoint;

        public Configuration(Point[] simplex, Actions action, Point centrePoint, Point reflectPoint, Point stretchPoint, Point сompressPoint)
        {
            this.simplex = simplex;
            this.action = action;
            this.centrePoint = centrePoint;
            this.reflectPoint = reflectPoint;
            this.stretchPoint = stretchPoint;
            this.сompressPoint = сompressPoint;
        }

        public static Configuration CreateConfiguration(NMconfig nmConfig)
        {
            Point[] newSimplex = Point.Clone(nmConfig.startSimplex);
            Array.Sort(newSimplex, new ValueIncreasingComparer());
            //Мне непонятно зачем нужен симпдекс из этого же объекта
            Point centrePoint = Centre(nmConfig.startSimplex);
            Point reflectPoint = Reflection(newSimplex, centrePoint, nmConfig.alpha, nmConfig.exp);
            Point stretchPoint = null;
            Point compressPoint = null;
            if ((newSimplex[0].value <= reflectPoint.value) && (reflectPoint.value <= newSimplex[nmConfig.n - 1].value)) //Случай 1 (отражение)
                return new Configuration(newSimplex, Actions.Reflection, centrePoint, reflectPoint, stretchPoint, null);
            else if (reflectPoint.value < newSimplex[0].value) //Случаи 1 и 2 (отражение или растяжение)
            {
                stretchPoint = Stretching(centrePoint, reflectPoint, nmConfig.beta, nmConfig.exp);
                if (stretchPoint.value < reflectPoint.value)
                    return new Configuration(newSimplex, Actions.Stretching, centrePoint, reflectPoint, stretchPoint, compressPoint);
                else
                    return new Configuration(newSimplex, Actions.Reflection, centrePoint, reflectPoint, stretchPoint, compressPoint);
            }
            else//случаи 3 и 4 (сжатие или глобальное сжатие)
            {
                compressPoint = Compression(newSimplex, centrePoint, reflectPoint, nmConfig.gamma, nmConfig.exp);
                if (compressPoint.value < Math.Min(newSimplex[nmConfig.n].value, reflectPoint.value))
                    return new Configuration(newSimplex, Actions.Compression, centrePoint, reflectPoint, stretchPoint, compressPoint);
                else
                    return new Configuration(newSimplex, Actions.GlobalCompression, centrePoint, reflectPoint, stretchPoint, compressPoint);
            }
        }

        public static Configuration NextConfiguration(Configuration conf,NMconfig nmConfig)
        {
            Point[] newSimplex = Point.Clone(conf.simplex);
            switch (conf.action)
            {
                case Actions.Reflection:
                    newSimplex[nmConfig.n] = conf.reflectPoint;
                    break;
                case Actions.Stretching:
                    newSimplex[nmConfig.n] = conf.stretchPoint;
                    break;
                case Actions.Compression:
                    newSimplex[nmConfig.n] = conf.сompressPoint;
                    break;
                case Actions.GlobalCompression:
                    GlobalComprerssion(newSimplex, nmConfig.exp);
                    break;
            }
            Array.Sort(newSimplex, new ValueIncreasingComparer());
            /*for (int i = 0; i < newSimplex.Length; i++)
            {
                Console.WriteLine($"simplex[{i}] = ");
                for (int j = 0; j < nmConfig.n; j++)
                    Console.WriteLine(newSimplex[i].coords[j]);
            }*/
            nmConfig.startSimplex = newSimplex;
            return CreateConfiguration(nmConfig);
        }
        static Point Centre(Point[] simplex)
        {
            int n = simplex[0].dimension;
            double[] point = new double[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    point[i] += simplex[j].coords[i];
                point[i] /= n;
            }
            return new Point(point);
        }
        static Point Reflection(Point[] simplex, Point centre, double alpha, Expression exp)
        {
            int n = simplex[0].dimension;
            double[] point = new double[n];
            for (int i = 0; i < n; i++)
                point[i] = centre.coords[i] + alpha * (centre.coords[i] - simplex[n].coords[i]);
            return new Point(point, exp);
        }
        static Point Stretching(Point centre, Point refPoint, double beta, Expression exp)
        {
            int n = centre.dimension;
            double[] point = new double[n];
            for (int i = 0; i < n; i++)
                point[i] = centre.coords[i] + beta * (refPoint.coords[i] - centre.coords[i]);
            return new Point(point, exp);
        }
        static Point Compression(Point[] simplex, Point centre, Point refPoint, double gamma, Expression exp)
        {
            int n = simplex[0].dimension;
            double[] point = new double[n];
            if (simplex[n].value <= refPoint.value)
                for (int i = 0; i < n; i++)
                    point[i] = centre.coords[i] + gamma * (simplex[n].coords[i] - centre.coords[i]);
            else
                for (int i = 0; i < n; i++)
                    point[i] = centre.coords[i] + gamma * (refPoint.coords[i] - centre.coords[i]);
            return new Point(point, exp);
        }
        static void GlobalComprerssion(Point[] simplex, Expression exp)
        {
            int n = simplex[0].dimension;
            double[] newPoint;
            for (int i = 1; i < n + 1; i++)
            {
                newPoint = new double[n];
                for (int j = 0; j < n; j++)
                    newPoint[j] = 0.5 * (simplex[i].coords[j] + simplex[0].coords[j]);
                simplex[i] = new Point(newPoint, exp);
            }
        }
    }

}
