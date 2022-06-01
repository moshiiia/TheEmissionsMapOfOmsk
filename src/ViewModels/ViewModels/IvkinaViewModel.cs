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
        public ObservableCollection<MapPolygon> Polygons { get; } = new();
        public string Colorsval { get; set; } 

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
        List<List<Cp_mass>> cp_Masses = new List<List<Cp_mass>>();

        double rangem = 100;
        double[] wR = new double[] { 6.4, 30.7, 21.1, 4.5, 10.9, 9.1, 6.8, 4.0 }; //роза ветров
        double fi; //угол в градусах
        double r; //расстояние
        double xist = 55.000302; //координаты источника(ТЭЦ-5 г.Омск)
        double yist = 73.487259;

        public static double calculateTheDistance(double latA, double lngA, double latB, double lngB)
        {
            //Радиус земли
            double earthRadius = 6372795;

            // перевести координаты в радианы
            double lat1 = latA * Math.PI / 180;
            double lng1 = lngA * Math.PI / 180;
            double lat2 = latB * Math.PI / 180;
            double lng2 = lngB * Math.PI / 180;

            //double dy = lat2 - lat1;
            //double dx = Math.Cos(Math.PI / 180 * lat1) * (lng2 - lng1);
            //double angel = Math.Atan2(dy, dx);
            //double gAngel = angel * 180 / Math.PI;

            // косинусы и синусы широт и разницы долгот
            double cl1 = Math.Cos(lat1);
            double cl2 = Math.Cos(lat2);
            double sl1 = Math.Sin(lat1);
            double sl2 = Math.Sin(lat2);
            double delta = lng2 - lng1;
            double cdelta = Math.Cos(delta);
            double sdelta = Math.Sin(delta);

            //double angel = Math.Atan2(cl1 * sdelta, cl1 * sl2 - sl1 * cl2 * cdelta);
            //double gAngel = angel * 180 / Math.PI;

            // вычисления длины большого круга
            double y = Math.Sqrt(Math.Pow(cl1 * sdelta, 2) + Math.Pow(cl1 * sl2 - sl1 * cl2 * cdelta, 2));
            double x = sl1 * sl2 + cl1 * cl2 * cdelta;

            double ad = Math.Atan2(y, x);
            double dist = ad * earthRadius;

            return dist;
        }
        public double CountFi(double fi)
        {
            if (fi < 0) fi = fi + 360;
            if (fi > 360) fi = fi - 360;
            return fi;
        }
        public double RoseFunc(double fi)
        {
            if (fi >= 0 && fi <= 45) return (wR[0] + (wR[1] - wR[0]) * (fi) / 45);
            else if (fi >= 45 && fi <= 90) return (wR[1] + (wR[2] - wR[1]) * (fi - 45) / 45);
            else if (fi >= 90 && fi < 135) return (wR[2] + (wR[3] - wR[2]) * (fi - 90) / 45);
            else if (fi >= 135 && fi <= 180) return (wR[3] + (wR[4] - wR[3]) * (fi - 135) / 45);
            else if (fi >= 180 && fi <= 225) return (wR[4] + (wR[5] - wR[4]) * (fi - 180) / 45);
            else if (fi >= 225 && fi <= 270) return (wR[5] + (wR[6] - wR[5]) * (fi - 225) / 45);
            else if (fi >= 270 && fi <= 315) return (wR[6] + (wR[7] - wR[6]) * (fi - 270) / 45);
            else if (fi >= 315 && fi <= 360) return (wR[7] + (wR[0] - wR[7]) * (fi - 315) / 45);
            else throw new Exception();
        }
        public (double, double) GeoToPol(double latitude, double longitude)//пересчитать х и у 
        {
            double earthRadius = 6371000; //Радиус земли км
            // перевести координаты в радианы
            double latist = xist * Math.PI / 180;
            double lngist = yist * Math.PI / 180;
            double latpoint = latitude * Math.PI / 180;
            double lngpoint = longitude * Math.PI / 180;

            // косинусы и синусы широт и разницы долгот
            double clist = Math.Cos(latist);
            double slist = Math.Sin(latist);
            double clpoint = Math.Cos(latpoint);
            double slpoint = Math.Sin(latpoint);
            double delta = lngpoint - lngist;
            double cdelta = Math.Cos(delta);
            double sdelta = Math.Sin(delta);

            double angle = Math.Atan2(clist * sdelta, clist * slpoint - slist * clpoint * cdelta);
            fi = angle * 180 / Math.PI; //в градусах
            // вычисления длины большого круга
            double y = Math.Sqrt(Math.Pow(clist * sdelta, 2) + Math.Pow(clist * slpoint - slist * clpoint * cdelta, 2));
            double x = slist * slpoint + clist * clpoint * cdelta;
            double ad = Math.Atan2(y, x);
            r = (ad * earthRadius)/1000; //в км  
            return new(r, fi);
        }

        /////////////////////// ММ2 по 3-ем точкам
        public void CountMathModel2(List<(double lat, double longitude, double amount)> points)
        {
            cp_Masses.Clear();
            double mnk = 0, mnk1 = 10000000000000;
            double Cp1buf = 0, Cp2buf = 0, Cp3buf = 0;
            double Cp1, Cp2, Cp3, Cp_point;
            double q1=0, q2=0, q3=0; 
            double tet1, tet2, tet3;

            for (tet1 = 1; tet1 < 1000; tet1 += 10)
            {
                for (tet2 = -2; tet2 < 2; tet2 += 0.01)
                {
                    for (tet3 = 0.1; tet3 < 1; tet3+=0.001)
                    {
                        var d = points[0].amount;
                        //(double x1, double y1) = GeoToDec(points[0].lat, points[0].longitude);
                        //(double r2, double fi2) = R_Fi_Count(x2, y2);
                       
                        (double r1, double fi1) = GeoToPol(points[0].lat, points[0].longitude);
                        (double r2, double fi2) = GeoToPol(points[1].lat, points[1].longitude);
                        (double r3, double fi3) = GeoToPol(points[2].lat, points[2].longitude);
                        
                        Cp1 = RoseFunc(CountFi(fi1)) * tet1 * Math.Pow(r1, tet2) * Math.Exp(-tet3 / r1);
                        Cp2 = RoseFunc(CountFi(fi2)) * tet1 * Math.Pow(r2, tet2) * Math.Exp(-tet3 / r2);
                        Cp3 = RoseFunc(CountFi(fi3)) * tet1 * Math.Pow(r3, tet2) * Math.Exp(-tet3 / r3);

                       mnk = Math.Pow(Cp1 - points[0].amount, 2) + Math.Pow(Cp2 - points[1].amount, 2) + Math.Pow(Cp3 - points[2].amount, 2);
                        if (mnk < mnk1)
                        {
                            mnk1 = mnk;
                            q1 = tet1; 
                            q2 = tet2;
                            q3 = tet3;
                            Cp1buf = Cp1;
                            Cp2buf = Cp2;
                            Cp3buf = Cp3;
                        }
                        mnk = 0;
                    }
                }
            }
            var dgfjgf = mnk1;

            double dist = calculateTheDistance(54.941745, 73.258271, 55.042831, 73.258271);
            double incrementX = Math.Abs((54.941745 - 55.042831) / (dist / rangem));
            dist = calculateTheDistance(54.941745, 73.258271, 54.941745, 73.598211);
            double incrementY = Math.Abs((73.258271 - 73.598211) / (dist / rangem));
            int numbx = 0;
            for (double x = 54.941745; x < 55.042831; x += incrementX)
            {
                cp_Masses.Add(new List<Cp_mass>());
                for (double y = 73.258271; y < 73.598211; y += incrementY)
                {
                    (double r_point, double fi_point) = GeoToPol(x, y);
                    Cp_point = RoseFunc(CountFi(fi_point)) * q1 * Math.Pow(r_point, q2) * Math.Exp(-q3 / r_point);
                    Cp_mass cp_Mass = new Cp_mass();
                    cp_Mass.Latitude = x;
                    cp_Mass.Longitude = y;
                    cp_Mass.Amount = Cp_point;
                    cp_Masses[numbx].Add(cp_Mass);
                }
                numbx++;
            }
            var b = cp_Masses.Count();
        }
        /////////////////////// ММ1 по 2-ум точкам проверить
        public void CountMathModel1(List<(double lat, double longitude, double amount)> points)
        {
            cp_Masses.Clear();
            double mnk = 0, mnk1 = 10000000000000;
            double Cp1buf = 0, Cp2buf = 0;
            double Cp1, Cp2, Cp_point;
            double q1=0, q2=0; 
            double tet1 = 0, tet2 = 0; 
            double rmax = 4.125; //км

            for (tet1 = 1; tet1 < 10000; tet1 += 1)
            {
                for (tet2 = -2; tet2 < 2; tet2 +=0.01)
                {
                    var d = points[0].amount; 
                    (double r1, double fi1) = GeoToPol(points[0].lat, points[0].longitude);
                    (double r2, double fi2) = GeoToPol(points[1].lat, points[1].longitude);

                    Cp1 = RoseFunc(CountFi(fi1)) * tet1 * Math.Pow(r1, tet2) * Math.Exp(-2 * rmax / r1);
                    Cp2 = RoseFunc(CountFi(fi2)) * tet1 * Math.Pow(r2, tet2) * Math.Exp(-2 * rmax / r2);

                    mnk = Math.Pow(Cp1 - points[0].amount, 2) + Math.Pow(Cp2 - points[1].amount, 2);

                    if (mnk < mnk1)
                    {
                        mnk1 = mnk;
                        q1 = tet1; 
                        q2 = tet2;
                        Cp1buf = Cp1;
                        Cp2buf = Cp2;
                    }
                    mnk = 0;
                }
            }
          
            double dist = calculateTheDistance(54.941745, 73.258271, 55.042831, 73.258271);
            double incrementX = Math.Abs((54.941745 - 55.042831) / (dist / rangem));
            dist = calculateTheDistance(54.941745, 73.258271, 54.941745, 73.598211);
            double incrementY = Math.Abs((73.258271 - 73.598211) / (dist / rangem));

            int numbx = 0;
            for (double x = 54.941745; x < 55.042831; x += incrementX)
            {
                cp_Masses.Add(new List<Cp_mass>());
                for (double y = 73.258271; y < 73.598211; y += incrementY)
                {
                    //переводим координаты для расчета
                    (double r_point, double fi_point) = GeoToPol(x, y);
                    Cp_point = RoseFunc(CountFi(fi_point)) * q1 * Math.Pow(r_point, q2) * Math.Exp(-2 * rmax / r_point);

                    Cp_mass cp_Mass = new Cp_mass();
                    cp_Mass.Latitude = x;
                    cp_Mass.Longitude = y;
                    cp_Mass.Amount = Cp_point;
                    //cp_Masses.Add(cp_Mass);
                    cp_Masses[numbx].Add(cp_Mass);
                }
                numbx++;
            }
            //var b = cp_Masses.Count();
        }
       
        public List<Location> red_Masses { get; set; } = new();
        public List<double> amounts { get; set; } = new();

        public void DrawPolygon(List<List<Cp_mass>> cp_Masses)
        {
            double minval=999999, maxval=0;
            Polygons.Clear();
            Colorsval="";

            List<Color> color = new List<Color>();
            color.Add(Colors.LightGreen);
            color.Add(Colors.GreenYellow);
            color.Add(Colors.Lime);
            color.Add(Colors.Olive);
            color.Add(Colors.Yellow);
            color.Add(Colors.DarkOrange);
            color.Add(Colors.Red);

            double dist = calculateTheDistance(54.941745, 73.258271, 55.042831, 73.258271);
            double incrementX = Math.Abs((54.941745 - 55.042831) / (dist / rangem));
            dist = calculateTheDistance(54.941745, 73.258271, 54.941745, 73.598211);
            double incrementY = Math.Abs((73.258271 - 73.598211) / (dist / rangem));
            foreach (List<Cp_mass> mass1 in cp_Masses)
            {
                foreach (Cp_mass cp_Mass in mass1)
                {
                    if (cp_Mass.Amount > maxval)
                    {
                        maxval = cp_Mass.Amount;
                    }
                    if (cp_Mass.Amount < minval)
                    {
                        minval = cp_Mass.Amount;
                    }
                }

            }
            double range = (maxval - minval) / color.Count;

            Colorsval = ": 0-"+Math.Round(range,2)+"\n: "+ Math.Round(range,2) +"-"+ Math.Round(range *2,2)+ "\n: " + Math.Round(range *2,2) + "-" + Math.Round(range * 3,2) 
                + "\n: "+ Math.Round(range *3,2) + "-" + Math.Round(range * 4,2);

            foreach (List<Cp_mass> mass1 in cp_Masses)
            {
                bool check = false;
                int colorbuf1 = -1;
                for (int i = 0; i < color.Count; i++)
                {
                    if (mass1[0].Amount <= minval + (range * (i + 1)))
                    {
                        colorbuf1 = i;
                        break;
                    }

                }
                if (colorbuf1 == -1)
                {
                    colorbuf1 = color.Count - 1;
                }

                double firstpos = mass1[0].Longitude;
                double lastpos = 0;
                foreach (Cp_mass cp_Mass in mass1)
                {
                    int colorbuf2 = -1;

                    for (int i = 0; i < color.Count; i++)
                    {
                        if (cp_Mass.Amount <= minval + (range * (i + 1)))
                        {
                            colorbuf2 = i;
                            break;
                        }

                    }
                    if (colorbuf2 == -1)
                    {
                        colorbuf2 = color.Count - 1;
                    }

                    if (colorbuf1 != colorbuf2)
                    {
                        MapPolygon polygon1 = new MapPolygon();
                        polygon1.Fill = new SolidColorBrush(color[colorbuf1]);
                        polygon1.Stroke = new SolidColorBrush(Colors.Green);
                        polygon1.StrokeThickness = 0;
                        polygon1.Opacity = 0.7;
                        polygon1.Locations = new LocationCollection() {
                                new Location(cp_Mass.Latitude, firstpos),
                                new Location(cp_Mass.Latitude+incrementX, firstpos),
                                new Location(cp_Mass.Latitude+incrementX, cp_Mass.Longitude+incrementY),
                                new Location(cp_Mass.Latitude, cp_Mass.Longitude+incrementY)};
                        Polygons.Add(polygon1);
                        firstpos = cp_Mass.Longitude+incrementY;
                        colorbuf1 = colorbuf2;
                    }
                    lastpos = cp_Mass.Latitude;

                    //for (int i = 0; i < color.Count; i++)
                    //{
                    //    if (cp_Mass.Amount <= minval + (range * (i + 1)))
                    //    {
                    //        MapPolygon polygon = new MapPolygon();
                    //        polygon.Fill = new SolidColorBrush(color[i]);
                    //        polygon.Stroke = new SolidColorBrush(Colors.Green);
                    //        polygon.StrokeThickness = 0;
                    //        polygon.Opacity = 0.7;
                    //        polygon.Locations = new LocationCollection() {
                    //        new Location(cp_Mass.Latitude, cp_Mass.Longitude),
                    //        new Location(cp_Mass.Latitude+incrementX, cp_Mass.Longitude),
                    //        new Location(cp_Mass.Latitude+incrementX, cp_Mass.Longitude+incrementY),
                    //        new Location(cp_Mass.Latitude, cp_Mass.Longitude+incrementY)};
                    //        Polygons.Add(polygon);
                    //        check = true;
                    //        break;
                    //    }

                    //}
                    //if (check == false)
                    //{
                    //    MapPolygon polygon = new MapPolygon();
                    //    polygon.Fill = new SolidColorBrush(color[color.Count - 1]);
                    //    polygon.Stroke = new SolidColorBrush(Colors.Green);
                    //    polygon.StrokeThickness = 0;
                    //    polygon.Opacity = 0.7;
                    //    polygon.Locations = new LocationCollection() {
                    //    new Location(cp_Mass.Latitude, cp_Mass.Longitude),
                    //    new Location(cp_Mass.Latitude+incrementX, cp_Mass.Longitude),
                    //    new Location(cp_Mass.Latitude+incrementX, cp_Mass.Longitude+incrementY),
                    //    new Location(cp_Mass.Latitude, cp_Mass.Longitude+incrementY)};
                    //    Polygons.Add(polygon);
                    //}
                }
                MapPolygon polygon = new MapPolygon();
                polygon.Fill = new SolidColorBrush(color[colorbuf1]);
                polygon.Stroke = new SolidColorBrush(Colors.Green);
                polygon.StrokeThickness = 0;
                polygon.Opacity = 0.7;
                polygon.Locations = new LocationCollection() {
                                new Location(lastpos, firstpos),
                                new Location(lastpos+incrementX, firstpos),
                                new Location(lastpos+incrementX, 73.598211+incrementY),
                                new Location(lastpos, 73.598211+incrementY)};
                Polygons.Add(polygon);
            }
            var b = amounts;
            OnPropertyChanged(nameof(red_Masses));
        }
    }
}

 


