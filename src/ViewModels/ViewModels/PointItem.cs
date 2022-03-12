using MapControl;
using MainModel.Entities;

namespace ViewModels
{
    public class PointItem
    {
        public string? Name { get; set; }
        public Location Location { get; set; } = null!;

        static internal PointItem  GetPoint(Point point)
        {
            PointItem item = new PointItem()
            {
                Name = point.Name,
                Location = new Location()
                {
                    Latitude = point.Coordinate.Latitude,
                    Longitude = point.Coordinate.Longitude
                }
            };

            //pushpin content = загрязнение. из бд
            return item;
        }
    }
}
