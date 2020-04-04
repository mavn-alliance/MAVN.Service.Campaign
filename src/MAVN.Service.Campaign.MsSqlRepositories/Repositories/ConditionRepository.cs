using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.MsSql;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Campaign.MsSqlRepositories.Repositories
{
    public class ConditionRepository : IConditionRepository
    {
        private readonly MsSqlContextFactory<CampaignContext> _msSqlContextFactory;
        private readonly IMapper _mapper;

        public ConditionRepository(
            MsSqlContextFactory<CampaignContext> msSqlContextFactory,
            IMapper mapper)
        {
            _msSqlContextFactory = msSqlContextFactory;
            _mapper = mapper;
        }

        public async Task<Guid> InsertAsync(Condition condition)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = _mapper.Map<ConditionEntity>(condition);

                context.Add(entity);

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task<Condition> GetConditionByIdAsync(Guid conditionId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.ConditionEntities
                    .Include(c => c.BonusTypeEntity)
                    .Include(c => c.ConditionPartners)
                    .Include(c => c.Attributes)
                    .SingleOrDefaultAsync(x => x.Id == conditionId);

                return _mapper.Map<Condition>(entity);
            }
        }

        public async Task<IReadOnlyCollection<Condition>> GetConditionsAsync()
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = await context.ConditionEntities.ToListAsync();

                return _mapper.Map<List<Condition>>(entities);
            }
        }

        public async Task<IReadOnlyCollection<Condition>> GetConditionsByCampaignIdAsync(Guid campaignId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = await context.ConditionEntities
                    .Where(e => e.CampaignEntityId == campaignId)
                    .ToListAsync();

                return _mapper.Map<List<Condition>>(entities);
            }
        }

        public async Task DeleteAsync(Condition condition)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = _mapper.Map<ConditionEntity>(condition);

                context.ConditionEntities.Remove(entity);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(IEnumerable<Condition> conditions)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = _mapper.Map<List<ConditionEntity>>(conditions);
                foreach (var entity in entities)
                {
                    context.ConditionEntities.Remove(entity);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyCollection<Condition>> GetConditionsForConditionTypeAsync(string bonusTypeName, bool? campaignActive)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = context.ConditionEntities
                    .Where(c => c.BonusTypeName.Equals(bonusTypeName)
                                && c.CampaignEntity.IsEnabled
                                && !c.CampaignEntity.IsDeleted);

                if (campaignActive.HasValue)
                {
                    var currentDate = DateTime.UtcNow;

                    if (campaignActive.Value)
                    {
                        entities = entities.Where(IsActiveCampaign(currentDate));
                    }
                    else
                    {
                        entities = entities.Where(IsNotActive(currentDate));
                    }
                }

                return _mapper.Map<List<Condition>>(await entities.ToListAsync());
            }
        }

        public async Task RemoveConditionPartnersAsync(Guid conditionId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = context.ConditionPartners.Where(cp => cp.ConditionEntityId == conditionId);

                context.ConditionPartners.RemoveRange(entities);

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteConditionAttributes(Guid conditionId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = context.ConditionAttributes
                    .Where(ca => ca.ConditionId == conditionId);

                context.ConditionAttributes.RemoveRange(entities);

                await context.SaveChangesAsync();
            }
        }

        private static Expression<Func<ConditionEntity, bool>> IsNotActive(DateTime date)
        {
            return entity => (entity.CampaignEntity.FromDate < date && entity.CampaignEntity.ToDate < date) &&
                             (entity.CampaignEntity.FromDate > date && entity.CampaignEntity.ToDate > date);
        }

        private static Expression<Func<ConditionEntity, bool>> IsActiveCampaign(DateTime date)
        {
            return entity => !entity.CampaignEntity.IsDeleted && entity.CampaignEntity.IsEnabled
                                      && entity.CampaignEntity.FromDate <= date
                                      && (entity.CampaignEntity.ToDate >= date || entity.CampaignEntity.ToDate.HasValue);
        }
    }
}
