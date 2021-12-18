using MainModel.Entities;
using MainModel.Entities.Enums;
using ParsersConsole;

var list = Parsers.NumLatLonVal_CoorPn(@"C:\Users\Root\source\repos\TheEmisssionsMapOfOmsk\materials\points.txt");

var dm = DataManager.Set(EfProvider.SqLite);



var pol = new Pollution()
{
    Id = Guid.NewGuid(),
    Name = "Пылевое загрязнение",
    MeasureUnit = MeasureUnit.mg_m2
};

await dm.Pollution.UpdateAsync(pol);
foreach (var (coor, pn) in list)
{
    await dm.Point.UpdateAsync(new Point
    {
        Coordinate = coor,
        PollutionSet = new PollutionSet
        {
            Amount = pn,
            Pollution = pol
        },
        Owner = Owner.Turkova
    });
}
Console.WriteLine("Done");