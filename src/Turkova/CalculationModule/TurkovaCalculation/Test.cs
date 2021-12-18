using MainModel.Entities;

namespace TurkovaCalculation;

public class Test
{
    public static async Task SetDate(PollutionSet pset) => await Task.Run(()=>pset.DateTime = DateTime.Now);
}