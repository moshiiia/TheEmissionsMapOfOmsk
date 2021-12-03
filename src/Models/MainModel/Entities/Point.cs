using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModel.Entities;

    public class Point
    {
    public Guid Id { get; init; }
    public Coordinate Сoordinate { get; set; } = null!;
    public IList<PollutionSet> PollutionSets { get; set; } = new List<PollutionSet>();
    
}

