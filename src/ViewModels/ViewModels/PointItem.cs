using MapControl;
using MainModel.Entities;

namespace ViewModels
{
    public class PointItem
    {
        public string? Name { get; set; }
        public Location Location { get; set; } = null!;
        public double Amount { get; set; }
        public string bindingText { get; set; } = null!;

        static internal PointItem  GetPoint(Point point)
        {
            PointItem item = new PointItem()
            {
                Name = point.Name,
                Location = new Location()
                {
                    Latitude = point.Coordinate.Latitude,
                    Longitude = point.Coordinate.Longitude
                },
                Amount = point.PollutionSet.Amount
            };
           
            return item;
        }

       
    }
}
