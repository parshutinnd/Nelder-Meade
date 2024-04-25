using System;
using System.Collections.Generic;
using FunctionParser;
using OptimizationMethods.Domain;
using OptimizationMethods.Model;

namespace OptimizationMethods.models
{
    enum Actions
    {
        Reflection,       //отражение
        Stretching,       //растяжение
        Compression,      //сжатие
        GlobalCompression //глобальное сжатие
    }
        class NelderMeade //алгоритм с параметрами
        {
            NMconfig nmConfig;
            public NelderMeade(NMconfig conf)
            {
               this.nmConfig = conf;
            }

            public List<Configuration> Run()
            {
                List<Configuration> configurations = new List<Configuration>(100);
                Point.CalculateValue(this.nmConfig.startSimplex, this.nmConfig.exp);

                var prevConfig = CreateConfiguration(this.nmConfig.startSimplex);
                configurations.Add(prevConfig);

                int i = 0;
                while (i < 100) 
                { 
                configurations.Add(NextConfiguration(prevConfig));
                i++;
                }
                
                return configurations;
            }
            public Configuration NextConfiguration(Configuration conf)
            {
                Point[] newSimplex = Point.Clone(conf.simplex);
                switch (conf.action)
                {
                    case Actions.Reflection:
                        newSimplex[this.nmConfig.n] = conf.reflectPoint;
                        break;
                    case Actions.Stretching:
                        newSimplex[this.nmConfig.n] = conf.stretchPoint;
                        break;
                    case Actions.Compression:
                        newSimplex[this.nmConfig.n] = conf.сompressPoint;
                        break;
                    case Actions.GlobalCompression:
                        GlobalComprerssion(newSimplex, this.nmConfig.exp);
                        break;
                }
                return CreateConfiguration(newSimplex);
            }
            public Configuration CreateConfiguration(Point[] simplex)
            {
                Point[] newSimplex = Point.Clone(simplex);
                Array.Sort(newSimplex, new ValueIncreasingComparer());
                Point centrePoint = Centre(simplex);
                Point reflectPoint = Reflection(newSimplex, centrePoint, this.nmConfig.alpha, this.nmConfig.exp);
                Point stretchPoint = null;
                Point compressPoint = null;
                if ((newSimplex[0].value <= reflectPoint.value) && (reflectPoint.value <= newSimplex[this.nmConfig.n - 1].value)) //Случай 1 (отражение)
                    return new Configuration(newSimplex, Actions.Reflection, centrePoint, reflectPoint, stretchPoint, null);
                else if (reflectPoint.value < newSimplex[0].value) //Случаи 1 и 2 (отражение или растяжение)
                {
                    stretchPoint = Stretching(centrePoint, reflectPoint, this.nmConfig.beta, this.nmConfig.exp);
                    if (stretchPoint.value < reflectPoint.value)
                        return new Configuration(newSimplex, Actions.Stretching, centrePoint, reflectPoint, stretchPoint, compressPoint);
                    else
                        return new Configuration(newSimplex, Actions.Reflection, centrePoint, reflectPoint, stretchPoint, compressPoint);
                }
                else//случаи 3 и 4 (сжатие или глобальное сжатие)
                {
                    compressPoint = Compression(newSimplex, centrePoint, reflectPoint, this.nmConfig.gamma, this.nmConfig.exp);
                    if (compressPoint.value < Math.Min(newSimplex[this.nmConfig.n].value, reflectPoint.value))
                        return new Configuration(newSimplex, Actions.Compression, centrePoint, reflectPoint, stretchPoint, compressPoint);
                    else
                        return new Configuration(newSimplex, Actions.GlobalCompression, centrePoint, reflectPoint, stretchPoint, compressPoint);
                }
            }
            bool StopCondition(Configuration conf)
            {
                return !(Dispersion(conf.simplex) < this.nmConfig.epsilon);
            }
            static double Dispersion(Point[] simplex)
            {
                int n = simplex[0].dimension;
                double disp = 0;
                for (int j = 1; j < n + 1; j++)
                    disp += Math.Pow(simplex[j].value - simplex[0].value, 2);
                disp /= n;
                return Math.Sqrt(disp);
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
