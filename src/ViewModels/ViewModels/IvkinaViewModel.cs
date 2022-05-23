using MainModel.Entities.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ViewModelBase.Commands;
using ViewModelBase.Commands.QuickCommands;
using MainModel.NotDbEntities;
using System.Windows.Shapes;
using System.Windows.Media;

using Microsoft.VisualBasic;

namespace ViewModels
{
    public class IvkinaViewModel : ViewModelBase.ViewModelBase
    {

        public ObservableCollection<PointItem> Points { get; set; }
        public ObservableCollection<PointItem> Pushpins { get; } = new();
        public ObservableCollection<PolylineItem> Polylines { get; } = new();
        public ObservableCollection<DataGridCont> DataGridConts { get; } = new();

        private const Owner Ivkina = Owner.Ivkina;
        private readonly DataManager data;
        public IErrorHandler? Handler { get; set; }

        private bool isBusy = false;
        public Command<PointItem> AddPointCommand { get; }

        public IvkinaViewModel()
        {
            CalculationCommand = new Command(AddCalculation, CanAddCalculation);

            data = DataManager.Set(EfProvider.SqLite);

            Points = new ObservableCollection<PointItem>(
                data.Point.Items.Where(i => i.Owner == Ivkina)
                .Select(p => PointItem.GetPoint(p)));

            Pushpins = new ObservableCollection<PointItem>(
                data.Point.Items.Where(i => i.Owner == Ivkina)
                .Select(p => PointItem.GetPoint(p)));

            DataGridConts = new ObservableCollection<DataGridCont>(
                data.Point.Items.Where(i => i.Owner == Ivkina)
                .Select(p => new DataGridCont()
                {
                    Num = p.Num,
                    PointName = p.Name,
                    Latitude = p.Coordinate.Latitude,
                    Longitude = p.Coordinate.Longitude,
                    PollutionName = p.PollutionSet.Pollution.Name,
                    PollutionAmount = p.PollutionSet.Amount
                }
                ).OrderBy(y => y.Num));
        }

        private bool CanAddCalculation()
        {
            var checks = Points.Where(y => y.IsSelected).Count();
            return checks is 2 or 3;
        }

        private void AddCalculation()
        {
            var checks = Points.Where(y => y.IsSelected);
            var count = checks.Count();

            var data = checks.Select(y => (y.Location.Latitude, y.Location.Longitude, y.Amount)).ToList();
            if (count == 3) {
                CountMathModel2(data);
                DrawPolygon();
            }
            //    else  CountMathModel1
        }

        public Command CalculationCommand { get; }
        public void RaiseCanCalculationCommand() => CalculationCommand.RaiseCanExecuteChanged();


        /////////////////////////////////////////////////////////////////// рассчеты
        public class Cp_mass
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Amount { get; set; }
        }
        
        List<Cp_mass> cp_Masses = new List<Cp_mass>();

        public ObservableCollection<PointItem> green_Masses { get; set; } = new();
        //public ObservableCollection<Point> yellow_Masses { get; set; } = new(); 
        //public ObservableCollection<Point> orange_Masses { get; set; } = new();
        //public ObservableCollection<Point> red_Masses { get; set; } = new();
        //PointCollection red_Masses = new PointCollection();

        double[] wR = new double[] { 6.8, 9.1, 10.9, 4.5, 21.1, 30.7, 6.4, 4.0 }; //роза ветров
        double fi; //угол в градусах
        double r; //расстояние
        double xist = -48.96545; //координаты источника(ТЭЦ-5 г.Омск)
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
        public (double, double) GeoToDec(double latitude, double longitude)
        {
            double a = 6378.1370; //экваториальный радиус конст
            double b = 6356.8; //полярный радиус конст

            double e = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2); //квадрат первого эксцентриситета эллипсоида                         
            double N = a / Math.Sqrt(1 - e * Math.Pow(Math.Sin(latitude), 2)); //радиус кривизны первого вертикала 

            //переход от географ.координат к декарт.
            double x = N * Math.Cos(latitude) * Math.Cos(longitude);
            double y = N * Math.Cos(latitude) * Math.Sin(longitude);
            return new(x, y);
        }
        public (double, double) R_Fi_Count(double x, double y)
        {

            //переход от декарт.координат к полярным
            r = Math.Sqrt(Math.Pow(x - xist, 2) + Math.Pow(y - yist, 2));
            fi = Math.Atan((y - yist) / (x - xist)) * 57.296;  //радианы или градусы??
            return new(r, fi);
        }


        /////////////////////// ММ2 по 3-ем точкам
        public void CountMathModel2(List<(double lat, double longitude, double amount)> points)
        {
            double mnk = 0, mnk1 = 10000000000000;
            double Cp1, Cp2, Cp3, Cp_point;
            double q1, q2, q3; //q для рассчета
            double Q1 = 0, Q2 = 0, Q3 = 0; //Q итоговые


            for (q1 = 1; q1 < 100; q1++)
            {
                for (q2 = 1; q2 < 100; q2++) //число растет
                {
                    for (q3 = 1; q3 < 100; q3++)
                    {
                        var d = points[0].amount;
                        (double x1, double y1) = GeoToDec(points[0].lat, points[0].longitude);
                        (double r1, double fi1) = R_Fi_Count(x1, y1);

                        (double x2, double y2) = GeoToDec(points[1].lat, points[1].longitude);
                        (double r2, double fi2) = R_Fi_Count(x2, y2);

                        (double x3, double y3) = GeoToDec(points[2].lat, points[2].longitude);
                        (double r3, double fi3) = R_Fi_Count(x3, y3);

                        Cp1 = CountFi(RoseFunc(fi1)) * q1 * Math.Pow(r1, q2) * Math.Exp(-q1 / r1);
                        Cp2 = CountFi(RoseFunc(fi2)) * q2 * Math.Pow(r2, q2) * Math.Exp(-q2 / r2);
                        Cp3 = CountFi(RoseFunc(fi3)) * q3 * Math.Pow(r3, q3) * Math.Exp(-q3 / r3);


                        mnk = Math.Pow(Cp1 - points[0].amount, 2) + Math.Pow(Cp2 - points[1].amount, 2) + Math.Pow(Cp3 - points[2].amount, 2);

                        if (mnk < mnk1)
                        {
                            mnk1 = mnk;
                            Q1 = q1; //почему значение не переприсваевается
                            Q2 = q2;
                            Q3 = q3;
                        }
                        mnk = 0;
                    }
                }
            }

                for (double x = 54.941745; x < 55.042831; x += 0.01)
                {
                    for (double y = 73.258271; y < 73.598211; y += 0.0001)
                    {
                        //переводим координаты в декартовые для расчета
                        (double xdec, double ydec) = GeoToDec(x, y);
                        (double r_point, double fi_point) = R_Fi_Count(xdec, ydec);
                        Cp_point = CountFi(RoseFunc(fi_point)) * Q1 * Math.Pow(r_point, Q2) * Math.Exp(-Q3 / r_point);

                        Cp_mass cp_Mass = new Cp_mass(); //создание нового элемента
                        cp_Mass.Latitude = x;
                        cp_Mass.Longitude = y;
                        cp_Mass.Amount = Cp_point;
                        cp_Masses.Add(cp_Mass); //создание в лист
                    }
                }
        }

            public void DrawPolygon()
            {
                Polygon green = new Polygon();
                green.Stroke = Brushes.Green;
                green.Fill = Brushes.Green;
                green.Opacity = 0.5;

                Polygon yellow = new Polygon();
                yellow.Stroke = Brushes.Yellow;
                yellow.Fill = Brushes.Yellow;
                yellow.Opacity = 0.5;

                Polygon orange = new Polygon();
                yellow.Stroke = Brushes.Orange;
                orange.Fill = Brushes.Orange;
                orange.Opacity = 0.5;

                Polygon red = new Polygon();
                red.Stroke = Brushes.Red;
                red.Fill = Brushes.Red;
                red.Opacity = 0.5;


                foreach (Cp_mass cp_Mass in cp_Masses)
                {
                    
                    if (cp_Mass.Amount < 150)
                    {
                    PointItem point = new PointItem();
                    point.Location.Latitude = cp_Mass.Latitude;
                    point.Location.Longitude=cp_Mass.Longitude;
                    green_Masses.Add(point);
                    }

                    if (cp_Mass.Amount >= 150 && cp_Mass.Amount < 200)
                    {
                        //yellow_Masses.Add(new Point(cp_Mass.Latitude, cp_Mass.Longitude));
                    }
                    if (cp_Mass.Amount >= 200 && cp_Mass.Amount < 300)
                    {
                        //orange_Masses.Add(new Point(cp_Mass.Latitude, cp_Mass.Longitude));
                    }
                    else
                    {
                        //red_Masses.Add(new Point(cp_Mass.Latitude, cp_Mass.Longitude));
                    }

                }

            //проверим строит ли полигон
            //green_Masses.Add(new Point(54.992616, 73.453983));
            //green_Masses.Add(new Point(55.006193, 73.513775));
            //green_Masses.Add(new Point(55.030132, 73.475767));

            //green.Points = green_Masses;
            //yellow.Points = yellow_Masses;
            //orange.Points = orange_Masses;
            //red.Points = red_Masses;
            }

        ///Расчетный модуль по 2 точкам (проверка на кол-во точек в функции во ViewModel)
        // вызываться будет либо Calc1 либо Calc2)
        ///Функция отрисовки полей отельно и вызывается после Calc 
    }
}

 


