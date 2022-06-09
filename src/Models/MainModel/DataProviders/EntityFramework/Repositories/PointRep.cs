using MainModel.Entities;
using Microsoft.EntityFrameworkCore;
using MainModel.Repositories;

namespace MainModel.DataProviders.EntityFramework.Repositories;

public class PointRep : IPoint
{
    private readonly EfDbContext context;
    public PointRep(EfDbContext context) => this.context = context;

    public IQueryable<Point> Items => context.Points.Include(p => p.PollutionSet).Include(p => p.Coordinate);

    public async Task DeleteAsync(Point point)
    {
        context.Remove(point);
        await context.SaveChangesAsync();
    }

    public Point? GetPollution(Guid id) => Items.FirstOrDefault(p => p.Id == id);

    public async Task UpdateAsync(Point point)
    {
        if (point.Id == default || !context.Points.Any(p=>p.Id==point.Id))
        {
            context.Add(point);
        }
        else 
        {
            if (GetPollution(point.Id) == null) throw new ArgumentNullException("Такой точки нет");
            context.Update(point);
        }
        await context.SaveChangesAsync();
    }

}