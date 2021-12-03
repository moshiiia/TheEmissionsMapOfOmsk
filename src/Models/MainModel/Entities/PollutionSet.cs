namespace MainModel.Entities;

public class PollutionSet
{
    public Guid Id { get; init; }
    public Point Point { get; set; } = null!;
    public double Amount { get; set; }//общее количество пыли в точке
    public Pollution Pollution { get; set; } = null!;
    public DateTime DateTime { get; set; }

}

