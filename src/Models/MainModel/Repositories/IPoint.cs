using MainModel.Entities;

namespace MainModel.Repositories;
public interface IPoint
{
    public IQueryable<Point> Items { get; }
    public Point? GetPollution(Guid id);
    
    public Task DeleteAsync(Point point);
    public Task UpdateAsync(Point point);

}

