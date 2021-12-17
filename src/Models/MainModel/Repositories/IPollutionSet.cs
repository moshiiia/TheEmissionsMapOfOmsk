using MainModel.Entities;

namespace MainModel.Repositories;
public interface IPollutionSet
{
    public IQueryable<PollutionSet> Items { get; }
    public IQueryable<PollutionSet> GetItemsByDate(DateTime dateTime);
    public Task<PollutionSet?> GetPollutionSetAsync(Guid id);
    public Task DeleteAsync(PollutionSet pollutionSet);
    public Task UpdateAsync(PollutionSet pollutionSet);

}

