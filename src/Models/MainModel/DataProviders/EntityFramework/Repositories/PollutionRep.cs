﻿using MainModel.Entities;
using MainModel.Repositories;

namespace MainModel.DataProviders.EntityFramework.Repositories;

public class PollutionRep : IPollution
{
    private readonly EfDbContext context;
    public PollutionRep(EfDbContext context) => this.context = context;
    public IQueryable<Pollution> Items => context.Pollutions;  //??

    public async Task DeleteAsync(Pollution pollution)
    {
        context.Remove(pollution);
        await context.SaveChangesAsync();
    }

    public Task <Pollution?> GetPollutionAsync(Guid id) => Task.Run(() => Items.FirstOrDefault(p => p.Id == id));

    public async Task UpdateAsync(Pollution pollution)
    {
        if (pollution.Id == default || !context.Pollutions.Any(p => p.Id == pollution.Id))
        {
            context.Add(pollution);
        }
        else
        {
            if ((await GetPollutionAsync(pollution.Id)) == null)
                context.Add(pollution);
            else
                context.Update(pollution);
        }
        await context.SaveChangesAsync();
    }
}