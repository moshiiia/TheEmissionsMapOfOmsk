using MainModel.Entities;
using MainModel.Repositories;

namespace MainModel.DataProviders.EntityFramework.Repositories;

public class PollutionSetRep : IPollutionSet
{
    private readonly EfDbContext context;
    public PollutionSetRep(EfDbContext context) => this.context = context;
    public IQueryable<PollutionSet> Items => context.PollutionSets;
    public IQueryable<PollutionSet> GetItemsByDate(DateTime dateTime)
        => Items.Where(p => p.DateTime == dateTime);

    public async Task DeleteAsync(PollutionSet pollutionSet)
    {
        context.Remove(pollutionSet);
        await context.SaveChangesAsync();
    }

    public Task<PollutionSet?> GetPollutionSetAsync(Guid id) => Task.Run(() => Items.FirstOrDefault(p => p.Id == id));

    public async Task UpdateAsync(PollutionSet pollutionSet)
    {
        if (pollutionSet.Id == default || !context.PollutionSets.Any(p => p.Id == pollutionSet.Id))
        {
            context.Add(pollutionSet);
        }
        else
        {
            if ((await GetPollutionSetAsync(pollutionSet.Id)) == null)
                context.Add(pollutionSet);
            else
                context.Update(pollutionSet);
        }
        await context.SaveChangesAsync();
    }

    
}