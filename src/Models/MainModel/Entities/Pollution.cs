using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModel.Entities;

   public class Pollution
    {
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; } //описание 
    public IList<PollutionSet> PollutionSets { get; set; } = new List<PollutionSet>(); //загрязнения в точках


}

