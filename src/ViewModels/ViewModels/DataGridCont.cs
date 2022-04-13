using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class DataGridCont
    {
        public int Num { get; init; }
        public string PointName { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public string PollutionName { get; init; }
        public double PollutionAmount { get; init; }

    }
}
