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
        public int Num { get; set; } 

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
                Num = point.Num
            };
           
            return item;
        }

       
    }
}
