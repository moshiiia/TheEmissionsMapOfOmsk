using MainModel.Entities;
using MainModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainModel.DataProviders.EntityFramework.Repositories
{
    public class PollutionSetRep : IPollutionSet
    {
        private readonly EfDbContext context;
        public PollutionSetRep(EfDbContext context) => this.context = context;
        public IQueryable<PollutionSet> Items => context.PollutionSets;
        public IQueryable<PollutionSet> GetItemsByDate(DateTime dateTime) 
            => Items.Where(p => p.DateTime == dateTime);

        public Task DeleteAsync(PollutionSet pollutionSet)
        {
            throw new NotImplementedException();
        }

        public Task<PollutionSet> GetPollutionSetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PollutionSet pollutionSet)
        {
            throw new NotImplementedException();
        }
    }
}
