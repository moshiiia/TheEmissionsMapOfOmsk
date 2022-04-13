using MainModel.Entities;
using MainModel.Entities.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;

namespace Tests;

[TestClass()]
public class DataManagerTests
{
    [TestMethod()]
    public void InsertPointsTest()
    {
        DataManager data = DataManager.Set(EfProvider.SqLite);
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Биофабрика,22",
            Owner = Owner.Ivkina,
            Num=2,
            Coordinate = new MainModel.Entities.Coordinate {
                Latitude = 54.992616,
                Longitude = 73.453983 },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 105,
            }
    }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Парковая,15",
            Owner = Owner.Ivkina,
            Num = 3,
            Coordinate = new MainModel.Entities.Coordinate {
                Latitude = 55.019382,
                Longitude = 73.574555
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 181
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Луговая,3а",
            Owner = Owner.Ivkina,
            Num = 4,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.016678, Longitude = 73.549609
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 261
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Луговая",
            Owner = Owner.Ivkina,
            Num = 5,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.015929,
                Longitude = 73.541012
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 302
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Левобережный разъезд,10",
            Owner = Owner.Ivkina,
            Num = 6,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.009373,
                Longitude = 73.530250
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 146
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Левобережный",
            Owner = Owner.Ivkina,
            Num = 7,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.009006,
                Longitude = 73.528067
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 138
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "снт Молния",
            Owner = Owner.Ivkina,
            Num = 8,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.006193,
                Longitude = 73.513775
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 150
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "10 лет октября 219к2Б",
            Owner = Owner.Ivkina,
            Num = 9,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.004540,
                Longitude = 73.498593
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 221
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "снт Любитель",
            Owner = Owner.Ivkina,
            Num = 10,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.040688,
                Longitude = 73.398952
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 206
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Завертяева,36",
            Owner = Owner.Ivkina,
            Num = 11,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.030132,
                Longitude = 73.475767
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 277
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "1-я Кожевенная",
            Owner = Owner.Ivkina,
            Num = 12,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.014881,
                Longitude = 73.494991
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 116
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "снт Союз",
            Owner = Owner.Ivkina,
            Num = 13,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.010147,
                Longitude = 73.486844
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 92
            }
        }).Wait();

        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Клевое Озеро",
            Owner = Owner.Ivkina,
            Num = 1,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.000931,
                Longitude = 73.4742044
            },
            PollutionSet = new MainModel.Entities.PollutionSet
            {
                Pollution = data.Pollution.Items.FirstOrDefaultAsync(p => p.Name == Pollution.Dust).Result ??
                throw new Exception("Пыли нет"),
                Amount = 117
            }
        }).Wait();
        Assert.IsTrue(true);
    }


    [TestMethod]
    public void InsertPollutionTest()
    {
        DataManager data = DataManager.Set(EfProvider.SqLite);
        data.Pollution.UpdateAsync(new MainModel.Entities.Pollution
        {
                Description = "частица минерального происхождения, диаметром > 0,45 мкм",
                MeasureUnit = MeasureUnit.mcg_m3
        }).Wait();
        Assert.IsTrue(true);
    }


}