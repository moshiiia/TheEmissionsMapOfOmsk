using MainModel.Entities;
using MainModel.Entities.Enums;
using ParsersConsole;

var list = Parsers.NumLatLonVal_CoorPn(@"D:\Downloads\Точки_отбора_ОМСК_2013_с_координатами_и_пылевая_нагрузка\points.txt");

var dm = DataManager.Set(EfProvider.SqLite);

var pol = new Pollution() {
    Id = Guid.NewGuid(),
    Name = "Пылевое загрязнение", 
    MeasureUnit = MeasureUnit.mg_m2};

await dm.Pollution.UpdateAsync(pol);
foreach (var (coor, pn) in list)
{
    await dm.PollutionSet.UpdateAsync(new PollutionSet
    {
        Amount = pn,
        DateTime = DateTime.Now,
        Point = new Point
        {
            Owner = Owner.Turkova,
            Coordinate = coor
        },
        Pollution = pol
    });
}