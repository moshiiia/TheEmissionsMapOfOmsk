using MainModel.Entities;

namespace MainModel.Repositories;
public interface IPollution
    {
        public IQueryable<Pollution> Items { get; }
        public Task<Pollution?> GetPollutionAsync(Guid id);
        public Task DeleteAsync(Pollution pollution);
        public Task UpdateAsync(Pollution pollution);
    }

