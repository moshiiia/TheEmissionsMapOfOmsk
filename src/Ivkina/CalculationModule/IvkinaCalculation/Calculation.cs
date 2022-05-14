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
            if (fi >= 0 && fi <= 45) return (wR[0] + (wR[1] - wR[0]) * (fi) / 45);
            else if (fi >= 45 && fi <= 90) return (wR[1] + (wR[2] - wR[3]) * (fi - 45) / 45);
            else if (fi >= 90 && fi < 135) return (wR[2] + (wR[3] - wR[2]) * (fi - 90) / 45);
            else if (fi >= 135 && fi <= 180) return (wR[3] + (wR[4] - wR[3]) * (fi - 135) / 45);
            else if (fi >= 180 && fi <= 225) return (wR[4] + (wR[5] - wR[4]) * (fi - 180) / 45);
            else if (fi >= 225 && fi <= 270) return (wR[5] + (wR[6] - wR[5]) * (fi - 225) / 45);
            else if (fi >= 270 && fi <= 315) return (wR[6] + (wR[7] - wR[6]) * (fi - 270) / 45);
            else if (fi >= 315 && fi <= 360) return (wR[7] + (wR[0] - wR[7]) * (fi - 315) / 45);
            else throw new Exception();
        }


        double xist = -48.96545; //координаты источника(ТЭЦ-5 г.Омск)
        double yist = -138.37523;

        public (double, double) GeoToPol(double latitude, double longitude) //преобразование координат
        {
            double a = 6378.1370; //экваториальный радиус
            double b = 6356.8; //полярный радиу

            double e = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2);  //квадрат первого эксцентриситета эллипсоида                         
            double N = a / Math.Sqrt(1 - e * Math.Pow(Math.Sin(latitude), 2)); //радиус кривизны первого вертикала 

            //переход от географ.координат к декарт.
            double x = N * Math.Cos(latitude) * Math.Cos(longitude);
            double y = N * Math.Cos(latitude) * Math.Sin(longitude);
            return new(x,y);
        }

        public (double,double) R_Fi_Count(double x,double y) 
        { 
            //переход от декарт.координат к полярным
            r = Math.Sqrt(Math.Pow(x - xist, 2) + Math.Pow(y - yist, 2));
            fi = Math.Atan((y - yist) / (x - xist));  //радианы или градусы??
            return new(r, fi);
        }


        /////////////////////// ММ2 по 3-ем точкам
        //public void CountMathModel2(List<PointItem> points)
        //{

        //    int mnk1 = 1000000;
        //    double point1val, point2val, point3val; //кол-во загрязнения в точке
        //    //передавать сюда точку из ListItem
        //    //доставать ее координаты и загрязнение для рассчетов

        //    int mnk = 0;
        //    double Cp1, Cp2,Cp3;
        //    double q1, q2, q3; //q для рассчета
        //    double Q1, Q2, Q3; //Q итоговые

        //    for (q1 = 0; q1< 10000; q1++)
        //    {
        //        for (q2 = 0; q2< 10000; q2++)
        //        {
        //            for (q3 = 0; q3< 10000; q3++)
        //            {


        //                double x1, y1 = GeoToPol(point1);
        //                double r1, fi1 = R_Fi_Count(x1, y1);

        //                double x2, y2 = GeoToPol(point2);
        //                double r2, fi2 = R_Fi_Count(x2, y2);

        //                double x3, y3 = GeoToPol(point3);
        //                double r3, fi3 = R_Fi_Count(x3, y3);

        //                Cp1 = CountFi(RoseFunc(fi1)) * q1* Math.Pow(r1, q2) * Math.Exp(-q1 / r1);
        //                Cp2 = CountFi(RoseFunc(fi2)) * q2* Math.Pow(r2, q2) * Math.Exp(-q2 / r2);
        //                Cp3 = CountFi(RoseFunc(fi3)) * q3* Math.Pow(r3, q3) * Math.Exp(-q3 / r3);

        //                mnk = Math.Pow(Cp1 - point1val, 2) + (Cp2 - point2val, 2) + (Cp3 - point3val, 2);

        //                if (mnk<mnk1)
        //                {
        //                    mnk1 = mnk;
        //                    Q1 = q1;
        //                    Q2 = q2;
        //                    Q3 = q3;
        //                }
        //             mnk1 = 0;
        //            }
        //        }
        //    }

           //полей для каждой точки области 

        //    for (double x = 125.82405; x > -94.38795; x -= 0.001)
        //    {
        //        for (double y = 196.66801; y > -404.54825; y -= 0.001)
        //        {
        //            double r_point, fi_point = R_Fi_Count(x,y);
        //            Cp_point = CountFi(RoseFunc(fi_point)) * Q1 * Math.Pow(r_point, Q2) * Math.Exp(-Q3 / r_point);
        //        //в массив записать Сp_point по ним потом строим интерпаляцию
        //        }
        //    }
        //}

        ////как отрисовывать поля после перевода? делать обратный перевод??
        ///
        ///Расчетный модуль по 2 точкам (проверка на кол-во точек в функции во ViewModel)
        // вызываться будет либо Calc1 либо Calc2)
        ///Функция отрисовки полей отельно и вызывается после Calc.
    }
}
