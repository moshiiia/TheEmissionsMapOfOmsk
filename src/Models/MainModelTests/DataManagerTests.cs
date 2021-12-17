using MainModel.Entities;
using MainModel.Entities.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Coordinate = new MainModel.Entities.Coordinate { 
                Latitude = 54.992616, 
                Longitude = 73.453983 }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Парковая,15",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate { 
                Latitude = 55.019382, 
                Longitude = 73.574555 
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Луговая,3а",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate 
            {
                Latitude = 55.016678, Longitude = 73.549609
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name= "Луговая",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.015929,
                Longitude = 73.541012
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Левобережный разъезд,10",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.009373,
                Longitude = 73.530250
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Левобережный",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.009006,
                Longitude = 73.528067
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "снт Молния",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.006193,
                Longitude = 73.513775
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "10 лет октября 219к2Б",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.004540,
                Longitude = 73.498593
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "снт Любитель",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.040688,
                Longitude = 73.398952
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Завертяева,36",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.030132,
                Longitude = 73.475767
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "1-я Кожевенная",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.014881,
                Longitude = 73.494991
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "снт Союз",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.010147,
                Longitude = 73.486844
            }
        }).Wait();
        data.Point.UpdateAsync(new MainModel.Entities.Point
        {
            Name = "Клевое Озеро",
            Owner = Owner.Ivkina,
            Coordinate = new MainModel.Entities.Coordinate
            {
                Latitude = 55.000931,
                Longitude = 73.4742044
            }
        }).Wait();
        Assert.IsTrue(true);
    }
}