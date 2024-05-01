using System;
using System.Collections.Generic;
using FunctionParser;

namespace OptimizationMethods
{
    class NelderMeade //алгоритм с параметрами
    {
        NMconfig nmConfig;
        public NelderMeade(NMconfig nmconf)
        {
            this.nmConfig = nmconf;
        }
        
        public List<Configuration> Run(NMconfig nmConfig)
        {
            List<Configuration> configurations = new List<Configuration>(100);
            Point.CalculateValue(nmConfig.startSimplex, nmConfig.exp);

            var Config = Configuration.CreateConfiguration(nmConfig);
            configurations.Add(Config);

            int i = 0;
            while (StopCondition(Config,nmConfig))
            {
                Console.Write($"{i}) {Dispersion(Config.simplex)}");
                Config = Configuration.NextConfiguration(Config, nmConfig);
                configurations.Add(Config);
                i++;
            }
            //Console.ReadLine();
            return configurations;
        }
        bool StopCondition(Configuration conf, NMconfig nmConfig)
        {
            return !(Dispersion(conf.simplex) < nmConfig.epsilon);
        }
        private double Dispersion(Point[] simplex)
        {
            int n = simplex[0].dimension;
            double disp = 0;
            for (int j = 1; j < n + 1; j++)
                disp += Math.Pow(simplex[j].value - simplex[0].value, 2);
            disp /= n;
            return Math.Sqrt(disp);
        }
    }
}
