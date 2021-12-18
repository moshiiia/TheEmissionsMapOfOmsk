
namespace MainModel.Entities;
public class Coordinate : EntityBase
{
    public double Latitude { get; set; } //широта
    public double Longitude { get; set; } //долгота

    public override string ToString() =>Latitude.ToString()+ "°, " + Longitude.ToString() + "°";


}

