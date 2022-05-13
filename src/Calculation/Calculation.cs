using MapControl;
using SampleApplication;
using System;
using System.Collections.Generic;

namespace Calculation
{
    public class Calculation
    {
        double[] wR = new double[] { 6.8, 9.1, 10.9, 4.5, 21.1, 30.7, 6.4, 4.0 }; //���� ������
        double fi; //���� � ��������
        double r; //����������
        double xist = -48.96545; //���������� ���������(���-5 �.����)
        double yist = -138.37523;

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

        public (double,double) GeoToPol(double latitude, double longitude)
            {
                double a = 6378.1370; //�������������� ������ �����
                double b = 6356.8; //�������� ������ �����

                double e = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2); //������� ������� ��������������� ����������                         
                double N = a / Math.Sqrt(1 - e * Math.Pow(Math.Sin(latitude), 2)); //������ �������� ������� ��������� 

                //������� �� �������.��������� � ������.
                double x = N * Math.Cos(latitude) * Math.Cos(longitude);
                double y = N * Math.Cos(latitude) * Math.Sin(longitude);
                return new(x, y);
            }

        public (double, double) R_Fi_Count(double x, double y)
            {
                //������� �� ������.��������� � ��������
                r = Math.Sqrt(Math.Pow(x - xist, 2) + Math.Pow(y - yist, 2));
                fi = Math.Atan((y - yist) / (x - xist));  //������� ��� �������??
                return new(r, fi);
            }


        /////////////////////// ��2 �� 3-�� ������
        public void CountMathModel2(List<PointItem> points)
        {
            double mnk = 0, mnk1 = 1000000;
            double Cp1, Cp2, Cp3, Cp_point;
            double q1, q2, q3; //q ��� ��������
            double Q1 = 0, Q2 = 0, Q3 = 0; //Q ��������
            List<double> Cp_mass = new List<double>();

            for (q1 = 0; q1 < 10000; q1++)
            {
                for (q2 = 0; q2 < 10000; q2++)
                {
                    for (q3 = 0; q3 < 10000; q3++)
                    {
                       (double x1, double y1) = GeoToPol(points[0].Location.Latitude, points[0].Location.Longitude);
                       (double r1, double fi1) = R_Fi_Count(x1, y1);

                       (double x2, double y2) = GeoToPol(points[1].Location.Latitude, points[1].Location.Longitude);
                       (double r2, double fi2) = R_Fi_Count(x2, y2);

                       (double x3, double y3) = GeoToPol(points[2].Location.Latitude, points[2].Location.Longitude);
                       (double r3, double fi3) = R_Fi_Count(x3, y3);

                       Cp1 = CountFi(RoseFunc(fi1)) * q1 * Math.Pow(r1, q2) * Math.Exp(-q1 / r1);
                       Cp2 = CountFi(RoseFunc(fi2)) * q2 * Math.Pow(r2, q2) * Math.Exp(-q2 / r2);
                       Cp3 = CountFi(RoseFunc(fi3)) * q3 * Math.Pow(r3, q3) * Math.Exp(-q3 / r3);

                       mnk = Math.Pow(Cp1 - points[0].Amount, 2) + Math.Pow(Cp2 - points[1].Amount, 2) + Math.Pow(Cp3 - points[3].Amount, 2);
                       if (mnk < mnk1)
                       {
                           mnk1 = mnk;
                           Q1 = q1;
                           Q2 = q2;
                           Q3 = q3;
                       }
                       mnk = 0;
                    }
                }
            }

             //����� ��� ������ ����� ������� 
            for (double x = 125.82405; x > -94.38795; x -= 0.001)
            {
                for (double y = 196.66801; y > -404.54825; y -= 0.001)
                {
                    (double r_point, double fi_point) = R_Fi_Count(x, y);
                    Cp_point = CountFi(RoseFunc(fi_point)) * Q1 * Math.Pow(r_point, Q2) * Math.Exp(-Q3 / r_point);
                    //� ���� �������� �p_point �� ��� ����� ������ ������������
                    Cp_mass.Add(Cp_point);
                }
            }
        }

            ////��� ������������ ���� ����� ��������? ������ �������� �������??
            ///
            ///��������� ������ �� 2 ������ (�������� �� ���-�� ����� � ������� �� ViewModel)
            // ���������� ����� ���� Calc1 ���� Calc2)
            ///������� ��������� ����� ������� � ���������� ����� Calc.

    }
}
