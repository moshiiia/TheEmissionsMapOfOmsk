using MainModel.Entities;

namespace MainModel.Repositories;
public interface ICoordinate
{
    public IQueryable<Coordinate> Items { get; }
    public Task<Coordinate?> GetCoordinateAsync(Guid id);
    public Task DeleteAsync(Coordinate coordinate);
    public Task UpdateAsync(Coordinate coordinate);


}

