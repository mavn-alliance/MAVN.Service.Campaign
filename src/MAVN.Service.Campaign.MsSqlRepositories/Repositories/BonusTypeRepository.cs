using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Common.MsSql;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Campaign.MsSqlRepositories.Repositories
{
    public class BonusTypeRepository : IBonusTypeRepository
    {
        private readonly MsSqlContextFactory<CampaignContext> _msSqlContextFactory;
        private readonly IMapper _mapper;

        public BonusTypeRepository(
            MsSqlContextFactory<CampaignContext>  msSqlContextFactory,
            IMapper mapper)
        {
            _msSqlContextFactory = msSqlContextFactory;
            _mapper = mapper;
        }

        public async Task<BonusType> InsertAsync(BonusType bonusType)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = _mapper.Map<BonusTypeEntity>(bonusType);

                context.Add((object)entity);
                await context.SaveChangesAsync();

                return _mapper.Map<BonusType>(entity);
            }
        }

        public async Task UpdateAsync(BonusType bonusType)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = _mapper.Map<BonusTypeEntity>(bonusType);

                context.BonusTypeEntities.Update(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<BonusType> GetBonusTypeAsync(string bonusTypeName)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                // Equals or == is translated to SQL, where case sensitivity depends
                // on the DB settings. To avoid problem in-memory comparison is used.
                var caseInsensitive = await context.BonusTypeEntities
                    .Where(c => c.Type.Equals(bonusTypeName))
                    .ToListAsync();

                var entity = caseInsensitive
                    .FirstOrDefault(c => c.Type == bonusTypeName);

                return _mapper.Map<BonusType>(entity);
            }
        }

        public async Task<BonusType> GetBonusTypeByDisplayNameAsync(string bonusTypeDisplayName)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.BonusTypeEntities
                    .FirstOrDefaultAsync(c => c.DisplayName.Equals(bonusTypeDisplayName));

                return _mapper.Map<BonusType>(entity);
            }
        }

        public async Task<IReadOnlyCollection<BonusType>> GetBonusTypesAsync()
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = await context.BonusTypeEntities.ToListAsync();

                return _mapper.Map<List<BonusType>>(entities);
            }
        }

        public async Task<IReadOnlyCollection<BonusType>> GetActiveBonusTypesAsync()
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = await context.BonusTypeEntities
                    .Where(e => e.IsAvailable)
                    .ToListAsync();

                return _mapper.Map<List<BonusType>>(entities);
            }
        }
    }
}
