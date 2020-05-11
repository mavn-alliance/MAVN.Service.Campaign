using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using MAVN.Common.MsSql;
using MAVN.Service.Campaign.Domain.Models.BurnRules;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using MAVN.Service.PartnerManagement.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Campaign.MsSqlRepositories.Repositories
{
    public class BurnRuleRepository : IBurnRuleRepository
    {
        private readonly MsSqlContextFactory<CampaignContext> _msSqlContextFactory;
        private readonly IMapper _mapper;

        public BurnRuleRepository(
            MsSqlContextFactory<CampaignContext> msSqlContextFactory,
            IMapper mapper)
        {
            _msSqlContextFactory = msSqlContextFactory ??
                                   throw new ArgumentNullException(nameof(msSqlContextFactory));
            _mapper = mapper;
        }

        public async Task<Guid> InsertAsync(BurnRuleModel burnRuleModel)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = _mapper.Map<BurnRuleEntity>(burnRuleModel);

                context.Add(entity);

                await context.SaveChangesAsync();

                return entity.Id;
            }
        }

        public async Task UpdateAsync(BurnRuleModel burnRuleModel)
        {
            var entity = _mapper.Map<BurnRuleEntity>(burnRuleModel);

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                // Skip updating the burn rule partners because we handle that separately 
                entity.BurnRulePartners = null;

                context.BurnRules.Update(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(BurnRuleModel burnRuleModel)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                burnRuleModel.IsDeleted = true;

                var entity = _mapper.Map<BurnRuleEntity>(burnRuleModel);
                context.BurnRules.Update(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<BurnRuleModel> GetAsync(Guid earnRuleId, bool includeDeleted = false)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.BurnRules
                    .Include(e => e.BurnRuleContents)
                    .Include(e => e.BurnRulePartners)
                    .FirstOrDefaultAsync(e => e.Id == earnRuleId && (includeDeleted || !e.IsDeleted));

                return _mapper.Map<BurnRuleModel>(entity);
            }
        }

        public async Task<PaginatedBurnRuleList> GetPagedAsync(BurnRuleListRequestModel request, bool includeContents)
        {
            var predicate = PredicateBuilder.New<BurnRuleEntity>(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace((request.Title)))
            {
                predicate = predicate.And(c => c.Title.ToLower()
                    .Contains(request.Title.Trim().ToLower()));
            }

            if (request.PartnerId != null)
            {
                predicate = predicate
                    .And(c => c.BurnRulePartners.Any(p => p.PartnerId == request.PartnerId));
            }

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var query = context.BurnRules.AsNoTracking();
                if (includeContents)
                    query = query
                        .Include(e => e.BurnRuleContents)
                        .Include(e => e.BurnRulePartners);

                var results = await query
                    .Where(predicate)
                    .ToListAsync();

                var total = results.Count;

                var skip = (request.CurrentPage - 1) * request.PageSize;

                results = results
                    .OrderByDescending(c =>
                    {
                        switch (c.Vertical)
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
                    })
                    .ThenBy(c => c.Order)
                    .ThenByDescending(c => c.CreationDate)
                    .Skip(skip)
                    .Take(request.PageSize)
                    .ToList();

                return new PaginatedBurnRuleList
                {
                    BurnRules = _mapper.Map<List<BurnRuleModel>>(results),
                    CurrentPage = request.CurrentPage,
                    PageSize = request.PageSize,
                    TotalCount = total
                };
            }
        }

        public async Task<BurnRuleModel> GetByIdAsync(Guid burnRuleId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.BurnRules
                    .Where(o => o.Id == burnRuleId)
                    .Include(e => e.BurnRuleContents)
                    .FirstOrDefaultAsync();

                return _mapper.Map<BurnRuleModel>(entity);
            }
        }

        public async Task<IReadOnlyList<BurnRuleModel>> GetByIdentifiersAsync(IReadOnlyList<Guid> identifiers)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = await context.BurnRules.AsNoTracking()
                    .Where(c => identifiers.Contains(c.Id))
                    .ToListAsync();

                return _mapper.Map<List<BurnRuleModel>>(entities);
            }
        }
    }
}
