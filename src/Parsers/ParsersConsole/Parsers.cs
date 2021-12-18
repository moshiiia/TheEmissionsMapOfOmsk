using System.Globalization;
using MainModel.Entities;

namespace ParsersConsole;

public static class Parsers
{
    public static IList<(Coordinate coor, double pn)> NumLatLonVal_CoorPn(string file)
    {
        const string err = "err";
        using var sr = new StreamReader(file);
        string line;
        var result = new List<(Coordinate, double)>();
        while ((line = sr?.ReadLine() ?? err) != null)
        {
            if (line == err) break;

            var unitsStrings = line.Split(' ');

            if (unitsStrings.Length != 4) continue;
            try
            {

                var lat = double.Parse(unitsStrings[1], CultureInfo.InvariantCulture);
                double lon = double.Parse(unitsStrings[2], CultureInfo.InvariantCulture);
                double pn = double.Parse(unitsStrings[3], CultureInfo.InvariantCulture);

                Coordinate coor = new() { Latitude = lat, Longitude = lon };

                result.Add(new ValueTuple<Coordinate, double>
                {
                    Item1 = coor,
                    Item2 = pn
                });
            }
            catch
            {
                continue;
            }
        }
        return result;
    }
}
