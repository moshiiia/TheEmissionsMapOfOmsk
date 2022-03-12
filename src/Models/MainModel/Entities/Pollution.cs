using MainModel.Entities.Enums;

namespace MainModel.Entities;

public class Pollution : EntityBase
{
    public const string Dust = "Пыль";
    public string Name { get; set; } = Dust;
    public string? Description { get; set; } //описание 

    public MeasureUnit MeasureUnit { get; set; } = MeasureUnit.Noname;

    public IList<PollutionSet> PollutionSets { get; set; } = new List<PollutionSet>(); //загрязнения в точках

}

