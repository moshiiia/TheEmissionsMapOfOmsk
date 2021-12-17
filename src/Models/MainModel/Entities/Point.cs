using MainModel.Entities.Enums;

namespace MainModel.Entities;

public class Point
{
    public Guid Id { get; init; }
    public string? Name { get; init; }  //имя объекта
    public Owner Owner { get; init; } = Owner.Noname; //к какому проекту относятся точки
    public Coordinate Coordinate { get; set; } = null!;
    public IList<PollutionSet> PollutionSets { get; set; } = new List<PollutionSet>();
}