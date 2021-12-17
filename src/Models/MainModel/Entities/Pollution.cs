using MainModel.Entities.Enums;

namespace MainModel.Entities;

   public class Pollution
    {
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } //описание 

    public MeasureUnit MeasureUnit { get; set; } = MeasureUnit.Noname;

    public IList<PollutionSet> PollutionSets { get; set; } = new List<PollutionSet>(); //загрязнения в точках


}

