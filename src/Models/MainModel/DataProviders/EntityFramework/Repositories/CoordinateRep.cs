using MainModel.Entities;
using MainModel.Repositories;

namespace MainModel.DataProviders.EntityFramework.Repositories;

public class CoordinateRep : ICoordinate
{
    private readonly EfDbContext context;
    public CoordinateRep(EfDbContext context) => this.context = context;

    public IQueryable<Coordinate> Items => context.Coordinates;  

    public async Task DeleteAsync(Coordinate coordinate)
    {
        context.Remove(coordinate);
        await context.SaveChangesAsync();
    }

    public Task <Coordinate?> GetCoordinateAsync(Guid id) =>Task.Run(()=>Items.FirstOrDefault(c => c.Id == id));

    public async Task UpdateAsync(Coordinate coordinate)
    {
        if (coordinate.Id == default)
        {
            context.Add(coordinate);
        }
        else
        {
            if (GetCoordinateAsync(coordinate.Id) == null) throw new ArgumentNullException("Такой координаты нет");
            context.Update(coordinate);
        }
        await context.SaveChangesAsync();
    }
}