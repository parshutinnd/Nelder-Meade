using OptimizationMethods.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationMethods.Domain
{
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


    }

}
