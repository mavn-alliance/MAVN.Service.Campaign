using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MAVN.Common.MsSql;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models.EarnRules;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Campaign.MsSqlRepositories.Repositories
{
    public class EarnRuleContentRepository : IEarnRuleContentRepository
    {
        private readonly MsSqlContextFactory<CampaignContext> _msSqlContextFactory;
        private readonly IMapper _mapper;

        public EarnRuleContentRepository(
            MsSqlContextFactory<CampaignContext> msSqlContextFactory,
            IMapper mapper)
        {
            _msSqlContextFactory = msSqlContextFactory ??
                throw new ArgumentNullException(nameof(msSqlContextFactory));
            _mapper = mapper;
        }

        public async Task DeleteAsync(IEnumerable<EarnRuleContentModel> spendRuleContents)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entities = _mapper.Map<List<EarnRuleContentEntity>>(spendRuleContents);

                foreach (var entity in entities)
                {
                    context.EarnRuleContents.Remove(entity);
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task<RuleContentType?> GetContentType(Guid ruleContentId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var type = await context.EarnRuleContents
                    .FirstOrDefaultAsync(e => e.Id == ruleContentId);

                return type?.RuleContentType;
            }
        }

        public async Task<EarnRuleContentModel> GetAsync(Guid contentId)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = await context.EarnRuleContents
                    .FirstOrDefaultAsync(e => e.Id == contentId);

                return _mapper.Map<EarnRuleContentModel>(entity);
            }
        }

        public async Task UpdateAsync(EarnRuleContentModel earnRuleContent)
        {
            using (var context = _msSqlContextFactory.CreateDataContext())
            {
                var entity = _mapper.Map<EarnRuleContentEntity>(earnRuleContent);
                context.EarnRuleContents.Update(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
