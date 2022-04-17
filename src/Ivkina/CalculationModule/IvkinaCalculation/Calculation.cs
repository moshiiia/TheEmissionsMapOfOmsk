using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvkinaCalculation
{
    public class Calculation
    {
        double[] wR = new double[] { 6.8, 9.1, 10.9, 4.5, 21.1, 30.7, 6.4, 4.0 }; //роза ветров
        double fi; //угол в градусах
        double r; //расстояние

        public double CountFi(double fi) 
        {
            if (fi < 0) fi = fi + 360;
            if (fi > 360) fi = fi - 360;
            return fi;
        }

       public double RoseFunc(double fi) 
        {
            if (fi>=0 && fi<=45) return (wR[0] + (wR[1] - wR[0]) * (fi) / 45);
            else if (fi >= 45 && fi <= 90) return(wR[1] + (wR[2] - wR[3]) * (fi - 45) / 45);
            else if (fi>= 90 && fi < 135) return (wR[2] + (wR[3] - wR[2]) * (fi - 90) / 45);
            else if (fi>= 135 && fi <= 180) return (wR[3] + (wR[4] - wR[3]) * (fi - 135) / 45);
            else if (fi>= 180 && fi <= 225) return (wR[4] + (wR[5] - wR[4]) * (fi - 180) / 45);
            else if (fi>= 225 && fi <= 270) return (wR[5] + (wR[6] - wR[5]) * (fi - 225) / 45);
            else if (fi>= 270 && fi <= 315) return (wR[6] + (wR[7] - wR[6]) * (fi - 270) / 45);
            else if(fi>= 315 && fi <= 360) return (wR[7] + (wR[0] - wR[7]) * (fi - 315) / 45);
            else throw new Exception();
        }

        public (double, double) GeoToPol(double latitude, double longitude) //преобразование координат
        { 
            double a = 6378.1370; //экваториальный радиус
            double b = 6356.8; //полярный радиус
            double xist = -48.96545; //координаты источника(ТЭЦ-5 г.Омск)
            double yist = -138.37523;

            double e = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);  //квадрат первого эксцентриситета эллипсоида                         
            double N = a / Math.Sqrt(1 - e * Math.Pow(Math.Sin(latitude), 2)); //радиус кривизны первого вертикала 
            
            //переход от географ. координат к декарт.
            double x = N * Math.Cos(latitude) * Math.Cos(longitude);
            double y = N * Math.Cos(latitude) * Math.Sin(longitude);
            
            //переход от декарт. координат к полярным 
            r = Math.Sqrt(Math.Pow(x-xist,2)+Math.Pow(y-yist,2));
            fi =Math.Atan((y-yist)/(x-xist));
            return new(r, fi);
        }
    }
}
