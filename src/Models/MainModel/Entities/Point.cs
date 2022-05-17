using MainModel.Entities.Enums;

namespace MainModel.Entities;

public class Point : EntityBase
{
    public Guid? PollutionSetId { get; set; }
    public Guid CoordinateId { get; set; }
    public string? Name { get; init; }  //имя объекта
    public Owner Owner { get; init; } = Owner.Noname; //к какому проекту относятся точки
    public Coordinate Coordinate { get; set; } = null!;
    public int Num { get; set; } //номер точки
    public PollutionSet? PollutionSet { get; set; } 
   
    public override string ToString() => Name == null ? String.Empty: Name + " " + Coordinate.ToString();

   
}