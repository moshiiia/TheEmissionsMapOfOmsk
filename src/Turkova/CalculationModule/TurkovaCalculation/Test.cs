using MainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurkovaCalculation
{
    public class Test
    {
        public static async Task SetDate(PollutionSet pset) => await Task.Run(()=>pset.DateTime = DateTime.Now);
    }
}
