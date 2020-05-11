using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using MAVN.Common.MsSql;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using MAVN.Service.PartnerManagement.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Campaign.MsSqlRepositories.Repositories
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly MsSqlContextFactory<CampaignContext> _msSqlContextFactory;
        private readonly IMapper _mapper;

        public CampaignRepository(
            MsSqlContextFactory<CampaignContext> msSqlContextFactory,
            IMapper mapper)
        {
            _msSqlContextFactory = msSqlContextFactory;
            _mapper = mapper;
        }

        public async Task<Guid> InsertAsync(CampaignDetails campaign)
        {
            var entity = _mapper.Map<CampaignEntity>(campaign);

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                context.Add(entity);

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task UpdateAsync(CampaignDetails campaign)
        {
            var entity = _mapper.Map<CampaignEntity>(campaign);

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                context.Campaigns.Update(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(CampaignDetails campaign)
        {
            campaign.IsDeleted = true;

            var entity = _mapper.Map<CampaignEntity>(campaign);

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                context.Campaigns.Update(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IReadOnlyCollection<CampaignDetails>> GetCampaignsAsync()
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = await context.Campaigns
                    .Include(c => c.EarnRuleContents)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.BonusTypeEntity)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.Attributes)
                    .Where(c => !c.IsDeleted)
                    .ToListAsync();

                return _mapper.Map<List<CampaignDetails>>(entities);
            }
        }

        public async Task<IReadOnlyCollection<CampaignDetails>> GetActiveCampaignsAsync()
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                // Current Date is passed as parameter to avoid execution in memory
                var currentDate = DateTime.UtcNow;
                var entities = await context.Campaigns
                    .Include(c => c.EarnRuleContents)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.BonusTypeEntity)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.Attributes)
                    .Where(c => !c.IsDeleted && c.IsEnabled)
                    .Where(IsActiveCampaign(currentDate))
                    .ToListAsync();

                return _mapper.Map<List<CampaignDetails>>(entities);
            }
        }

        public async Task<CampaignDetails> GetCampaignAsync(Guid campaignId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.Campaigns
                    .Include(c => c.ConditionEntities)
                        .ThenInclude(s => s.BonusTypeEntity)
                    .Include(c => c.ConditionEntities)
                        .ThenInclude(ce => ce.ConditionPartners)
                    .Include(c => c.ConditionEntities)
                        .ThenInclude(ce => ce.Attributes)
                    .Include(c => c.EarnRuleContents)
                    .FirstOrDefaultAsync(c => !c.IsDeleted && c.Id == campaignId);

                return _mapper.Map<CampaignDetails>(entity);
            }
        }

        public async Task<IReadOnlyCollection<CampaignDetails>> GetAllWithConditionTypeAsync(string conditionType, bool active)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                // Current Date is passed as parameter to avoid execution in memory
                var currentDate = DateTime.UtcNow;
                var entities = await context.Campaigns
                    .Include(c => c.EarnRuleContents)
                    .Include(c => c.ConditionEntities)
                    .Where(c => c.ConditionEntities.Any(e => e.BonusTypeName == conditionType.ToLower()))
                    .Where(c => !c.IsDeleted && c.IsEnabled)
                    .Where(IsActiveCampaign(currentDate))
                    .ToListAsync();

                return _mapper.Map<List<CampaignDetails>>(entities);
            }
        }

        public async Task<PaginatedCampaignListModel> GetEnabledCampaignsByStatusAsync(List<CampaignStatus> statuses, PaginationModel pagination)
        {
            var stats = statuses.Distinct();

            var innerPredicate = PredicateBuilder.New<CampaignEntity>(false);
            var outerPredicate = PredicateBuilder.New<CampaignEntity>(true);

            innerPredicate = GetMultipleStatusesFilter(stats, innerPredicate);

            outerPredicate = outerPredicate.And(c => !c.IsDeleted && c.IsEnabled);
            outerPredicate = outerPredicate.And(innerPredicate);

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var query = context.Campaigns
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(s => s.BonusTypeEntity)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.ConditionPartners)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.Attributes)
                    .Include(c => c.EarnRuleContents)
                    .Where(outerPredicate);

                var totalCount = await query.CountAsync();

                var skip = (pagination.CurrentPage - 1) * pagination.PageSize;

                var entities = await query.ToListAsync();

                entities = entities.OrderByDescending(OrderCampaignByVertical)
                    .ThenBy(c => c.Order)
                    .ThenByDescending(c => c.CreationDate)
                    .Skip(skip)
                    .Take(pagination.PageSize)
                    .ToList();

                return new PaginatedCampaignListModel
                {
                    Campaigns = _mapper.Map<IEnumerable<CampaignDetails>>(entities),
                    CurrentPage = pagination.CurrentPage,
                    PageSize = pagination.PageSize,
                    TotalCount = totalCount
                };
            }
        }

        private static ExpressionStarter<CampaignEntity> GetMultipleStatusesFilter(IEnumerable<CampaignStatus> stats, ExpressionStarter<CampaignEntity> innerPredicate)
        {
            var currentDate = DateTime.UtcNow;

            foreach (var status in stats)
            {
                switch (status)
                {
                    case CampaignStatus.Pending:
                        innerPredicate = innerPredicate.Or(IsPendingCampaign(currentDate).And(c => c.IsEnabled));
                        break;
                    case CampaignStatus.Active:
                        innerPredicate = innerPredicate.Or(IsActiveCampaign(currentDate).And(c => c.IsEnabled));
                        break;
                    case CampaignStatus.Completed:
                        innerPredicate = innerPredicate.Or(IsCompletedCampaign(currentDate).And(c => c.IsEnabled));
                        break;
                    case CampaignStatus.Inactive:
                        innerPredicate = innerPredicate.Or(IsInactiveCampaign());
                        break;
                }
            }

            return innerPredicate;
        }

        public async Task<PaginatedCampaignListModel> GetPagedCampaignsAsync(CampaignListRequestModel campaignListRequestModel)
        {
            Expression<Func<CampaignEntity, bool>> predicate = PredicateBuilder.New<CampaignEntity>(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(campaignListRequestModel.CampaignName))
            {
                predicate = predicate.And(c => c.Name.ToLower().Contains(campaignListRequestModel.CampaignName.Trim().ToLower()));
            }

            if (campaignListRequestModel.CampaignStatus != null)
            {
                predicate = GetFilterByCampaignStatus(campaignListRequestModel.CampaignStatus.Value, predicate);
            }

            if (!string.IsNullOrEmpty(campaignListRequestModel.ConditionType))
            {
                predicate = predicate.And(c => c.ConditionEntities.Any(e => e.BonusTypeName == campaignListRequestModel.ConditionType.ToLower()));
            }

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var query = context.Campaigns
                    .AsNoTracking()
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.BonusTypeEntity)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.ConditionPartners)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.Attributes)
                    .Where(predicate);

                var totalCount = await query.CountAsync();
                var skip = (campaignListRequestModel.CurrentPage - 1) * campaignListRequestModel.PageSize;

                var result = await query.ToListAsync();

                result = result
                    .OrderByDescending(OrderCampaignByVertical)
                    .ThenBy(c => c.Order)
                    .ThenByDescending(c => c.CreationDate)
                    .Skip(skip)
                    .Take(campaignListRequestModel.PageSize)
                    .ToList();

                return new PaginatedCampaignListModel
                {
                    Campaigns = _mapper.Map<List<CampaignDetails>>(result),
                    CurrentPage = campaignListRequestModel.CurrentPage,
                    PageSize = campaignListRequestModel.PageSize,
                    TotalCount = totalCount
                };
            }
        }

        public async Task<IReadOnlyList<CampaignDto>> GetAllByStatus(CampaignStatus status, DateTime date)
        {
            Expression<Func<CampaignEntity, bool>> predicate = PredicateBuilder.New<CampaignEntity>(true);

            predicate = GetFilterByCampaignStatus(status, predicate, date, true, false);

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var result = await context.Campaigns
                    .AsNoTracking()
                    .Where(predicate)
                    .ToListAsync();

                return _mapper.Map<IReadOnlyList<CampaignDto>>(result);
            }
        }

        public async Task<CampaignDetails> GetByIdAsync(Guid campaignId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.Campaigns
                    .Where(o => o.Id == campaignId)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(s => s.BonusTypeEntity)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.ConditionPartners)
                    .Include(c => c.ConditionEntities)
                    .ThenInclude(ce => ce.Attributes)
                    .Include(c => c.EarnRuleContents)
                    .FirstOrDefaultAsync();

                return _mapper.Map<CampaignDetails>(entity);
            }
        }

        private Expression<Func<CampaignEntity, bool>> GetFilterByCampaignStatus(CampaignStatus campaignStatus,
            Expression<Func<CampaignEntity, bool>> predicate, DateTime? currentDate = null, bool includeDeleted = false, bool onlyEnabled = true)
        {
            if (!currentDate.HasValue)
                currentDate = DateTime.UtcNow;

            if (!includeDeleted)
                predicate = predicate.And(c => !c.IsDeleted);

            if (onlyEnabled && campaignStatus != CampaignStatus.Inactive)
                predicate = predicate.And(c => c.IsEnabled);

            switch (campaignStatus)
            {
                case CampaignStatus.Pending:
                    predicate = predicate.And(IsPendingCampaign(currentDate.Value));
                    break;
                case CampaignStatus.Active:
                    predicate = predicate.And(IsActiveCampaign(currentDate.Value));
                    break;
                case CampaignStatus.Completed:
                    predicate = predicate.And(IsCompletedCampaign(currentDate.Value));
                    break;
                case CampaignStatus.Inactive:
                    predicate = predicate.And(IsInactiveCampaign());
                    break;
            }

            return predicate;
        }

        public async Task<IReadOnlyCollection<CampaignDetails>> GetCampaignsByIdsAsync(Guid[] campaignIds)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var query = await context.Campaigns.AsNoTracking()
                    .Where(c => campaignIds.Contains(c.Id))
                    .ToListAsync();

                return _mapper.Map<List<CampaignDetails>>(query);
            }
        }

        private static Expression<Func<CampaignEntity, bool>> IsActiveCampaign(DateTime currentDate)
        {
            return campaignEntity => campaignEntity.FromDate <= currentDate
                                     && (!campaignEntity.ToDate.HasValue || campaignEntity.ToDate.Value > currentDate);
        }

        private static Expression<Func<CampaignEntity, bool>> IsPendingCampaign(DateTime currentDate)
        {
            return campaignEntity => campaignEntity.FromDate > currentDate;
        }

        private static Expression<Func<CampaignEntity, bool>> IsCompletedCampaign(DateTime currentDate)
        {
            return campaignEntity => campaignEntity.ToDate.HasValue && campaignEntity.ToDate.Value < currentDate;
        }

        private static Expression<Func<CampaignEntity, bool>> IsInactiveCampaign()
        {
            return campaignEntity => !campaignEntity.IsEnabled;
        }

        private static int OrderCampaignByVertical(CampaignEntity c)
        {
            var condition = c.ConditionEntities.FirstOrDefault();

            if (condition?.BonusTypeEntity == null)
            {
                return 0;
            }

            switch (condition?.BonusTypeEntity.Vertical)
            {
                case Vertical.RealEstate:
                    return 3;
                case Vertical.Hospitality:
                    return 2;
                case Vertical.Retail:
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
