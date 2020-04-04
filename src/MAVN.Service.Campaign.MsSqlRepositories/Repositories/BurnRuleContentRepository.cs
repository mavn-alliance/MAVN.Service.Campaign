using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Common.MsSql;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models.BurnRules;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Campaign.MsSqlRepositories.Repositories
{
    public class BurnRuleContentRepository : IBurnRuleContentRepository
    {
        private readonly MsSqlContextFactory<CampaignContext> _msSqlContextFactory;
        private readonly IMapper _mapper;

        public BurnRuleContentRepository(
            MsSqlContextFactory<CampaignContext> msSqlContextFactory,
            IMapper mapper)
        {
            _msSqlContextFactory = msSqlContextFactory ??
                                   throw new ArgumentNullException(nameof(msSqlContextFactory));
            _mapper = mapper;
        }

        public async Task DeleteAsync(IEnumerable<BurnRuleContentModel> burRuleContents)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = _mapper.Map<List<BurnRuleContentEntity>>(burRuleContents);

                foreach (var entity in entities)
                {
                    context.BurnRuleContents.Remove(entity);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<RuleContentType?> GetContentType(Guid ruleContentId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var type = await context.BurnRuleContents
                    .FirstOrDefaultAsync(e => e.Id == ruleContentId);

                return type?.RuleContentType;
            }
        }

        public async Task<BurnRuleContentModel> GetAsync(Guid contentId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.BurnRuleContents
                    .FirstOrDefaultAsync(e => e.Id == contentId);

                return _mapper.Map<BurnRuleContentModel>(entity);
            }
        }

        public async Task UpdateAsync(BurnRuleContentModel burnRuleContent)
        {
            var entity = _mapper.Map<BurnRuleContentEntity>(burnRuleContent);

            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                context.BurnRuleContents.Update(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
