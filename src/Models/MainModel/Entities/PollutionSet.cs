namespace MainModel.Entities;

public class PollutionSet : EntityBase
{
    public Guid PollutionId { get; set; }
    //public Guid PointId { get; set; }
    //public Point Point { get; set; } = null!;
   // public Owner Owner { get; init; } = Owner.Noname;
    public double Amount { get; set; }//общее количество пыли в точке
    public Pollution Pollution { get; set; } = null!;
    public DateTime? DateTime { get; set; }

}

