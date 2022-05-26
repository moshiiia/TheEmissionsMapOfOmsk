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
using System.Windows;
using MapControl;

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
                DrawPolygon(cp_Masses);
            }
            if (count == 2)
            {
                CountMathModel1(data);
                DrawPolygon(cp_Masses);
            }
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

            for (double x = 54.941745; x < 55.042831; x += 0.1)
            {
                for (double y = 73.258271; y < 73.598211; y += 0.001)
                {
                    //переводим координаты в декартовые для расчета
                    (double xdec, double ydec) = GeoToDec(x, y);
                    (double r_point, double fi_point) = R_Fi_Count(xdec, ydec);
                    Cp_point = CountFi(RoseFunc(fi_point)) * Q1 * Math.Pow(r_point, Q2) * Math.Exp(-Q3 / r_point);

                    Cp_mass cp_Mass = new Cp_mass();
                    cp_Mass.Latitude = x;
                    cp_Mass.Longitude = y;
                    cp_Mass.Amount = Cp_point;
                    cp_Masses.Add(cp_Mass);
                }
            }
            //var b = cp_Masses.Count();
        }

        /////////////////////// ММ1 по 2-ум точкам
        public void CountMathModel1(List<(double lat, double longitude, double amount)> points)
        {
            double mnk = 0, mnk1 = 10000000000000;
            double Cp1, Cp2, Cp_point;
            double q1, q2; //q для рассчета
            double Q1 = 0, Q2 = 0; //Q итоговые
            double rmax = 4.125; //км

            for (q1 = 1; q1 < 100; q1++)
            {
                for (q2 = 1; q2 < 100; q2++) //число растет
                {
                    var d = points[0].amount;
                    (double x1, double y1) = GeoToDec(points[0].lat, points[0].longitude);
                    (double r1, double fi1) = R_Fi_Count(x1, y1);

                    (double x2, double y2) = GeoToDec(points[1].lat, points[1].longitude);
                    (double r2, double fi2) = R_Fi_Count(x2, y2);

                    Cp1 = CountFi(RoseFunc(fi1)) * q1 * Math.Pow(r1, q2) * Math.Exp(-2 * rmax / r1);
                    Cp2 = CountFi(RoseFunc(fi2)) * q2 * Math.Pow(r2, q2) * Math.Exp(-2 * rmax / r2);

                    mnk = Math.Pow(Cp1 - points[0].amount, 2) + Math.Pow(Cp2 - points[1].amount, 2);

                    if (mnk < mnk1)
                    {
                        mnk1 = mnk;
                        Q1 = q1; //почему значение не переприсваевается
                        Q2 = q2;
                    }
                    mnk = 0;
                }
            }

            for (double x = 54.941745; x < 55.042831; x += 0.1)
            {
                for (double y = 73.258271; y < 73.598211; y += 0.001)
                {
                    //переводим координаты в декартовые для расчета
                    (double xdec, double ydec) = GeoToDec(x, y);
                    (double r_point, double fi_point) = R_Fi_Count(xdec, ydec);
                    Cp_point = CountFi(RoseFunc(fi_point)) * Q1 * Math.Pow(r_point, Q2) * Math.Exp(-2 * rmax / r_point);

                    Cp_mass cp_Mass = new Cp_mass();
                    cp_Mass.Latitude = x;
                    cp_Mass.Longitude = y;
                    cp_Mass.Amount = Cp_point;
                    cp_Masses.Add(cp_Mass);
                }
            }
            //var b = cp_Masses.Count();
        }


        //public List<Location> green_Masses { get; set; } = new();
        //public List<Location> yellow_Masses { get; set; } = new();
        //public List<Location> orange_Masses { get; set; } = new();
        public List<Location> red_Masses { get; set; } = new();

        //public List<MapPath> red_Item { get; set; } = new();
        public void DrawPolygon(List<Cp_mass> cp_Masses)
        {
            //green_Masses.Clear();
            //yellow_Masses.Clear();
            //orange_Masses.Clear();
            red_Masses.Clear();

            // double red_Min=0, red_Max=0;

            //red_Item.Clear();
            foreach (Cp_mass cp_Mass in cp_Masses)
            {

                ICollection<Location>? collection = null;

                //if (cp_Mass.Amount < 2000)
                //{
                //    collection = green_Masses;
                //}

                //if (cp_Mass.Amount >= 2000 && cp_Mass.Amount < 4200)
                //{
                //    collection = yellow_Masses;
                //}

                //if (cp_Mass.Amount >= 4200 && cp_Mass.Amount < 4900)
                //{
                //    collection = orange_Masses;
                //}

                //ICollection<MapPath>? collection = null;
                if (cp_Mass.Amount < 5000)
                {
                    //collection = red_Item;
                    //int radius = 1000;
                    //EllipseGeometry ellipse = new EllipseGeometry();
                    //ellipse.RadiusX = radius;
                    //ellipse.RadiusY = radius;

                    //MapPath mapPath = new MapPath();
                    //mapPath.Location = new Location(cp_Mass.Latitude,cp_Mass.Longitude);
                    //mapPath.Data = ellipse;
                    //red_Item.Add(mapPath);
                }
                Location point = new(cp_Mass.Latitude, cp_Mass.Longitude);

                //if (collection is red_Masses)
                //{
                //    if (point.Longitude < red_Min)
                //    { red_Min = point.Longitude;
                //       //вставить в начало коллекции
                //    }
                //}

                collection?.Add(point);
            }
            //EllipseGeometry ellipse1 = new EllipseGeometry();
            //ellipse1.RadiusX = 10000;
            //ellipse1.RadiusY = 10000;
            //MapPath mapPath1 = new MapPath();
            //mapPath1.Location = new Location(-48.96545, -138.37523);
            //mapPath1.Data = ellipse1;
            //red_Item.Add(mapPath1);

            //OnPropertyChanged(nameof(green_Masses));
            //OnPropertyChanged(nameof(yellow_Masses));
            //OnPropertyChanged(nameof(orange_Masses));

            Location point1 = new(-48.96545, -138.37523);
            red_Masses.Add(point1);
            OnPropertyChanged(nameof(red_Masses));
            //OnPropertyChanged(nameof(red_Item));
        }
    }
}

 


