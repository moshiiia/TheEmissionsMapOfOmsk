using MainModel.Entities;
using MapControl;
namespace ViewModels
{
    public class PointItem
    {
        public string? Name { get; set; }
        public Location Location { get; set; } = null!;
        public double Amount { get; set; } 
        public string bindingText { get; set; } = null!;
        public int Num { get; set; }
        public bool IsSelected { get; set; }
       
        public static PointItem  GetPoint(Point point)
        {
            PointItem item = new PointItem()
            {
                Name = point.Name,
                Location = new Location()
                {
                    Latitude = point.Coordinate.Latitude,
                    Longitude = point.Coordinate.Longitude
                },
                Num = point.Num,
                Amount=point.PollutionSet.Amount
            };
           
            return item;
        }
    }
}
