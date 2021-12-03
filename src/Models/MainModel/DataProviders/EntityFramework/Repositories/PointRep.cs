using MainModel.Entities;
using MainModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModel.DataProviders.EntityFramework.Repositories
{
    public class PointRep : IPoint
    {
        private readonly EfDbContext context;
        public PointRep(EfDbContext context) => this.context = context;

        public IQueryable<Coordinate> Items => context.Coordinates;

        public async Task DeleteAsync(Point point)
        {
            context.Remove(point);
            await context.SaveChangesAsync();
        }

        public Point? GetPollution(Guid id) => Items.FirstOrDefault(p => p.Id == id);

        public async Task UpdateAsync(Point point)
        {
            if (point.Id == default)
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
}
