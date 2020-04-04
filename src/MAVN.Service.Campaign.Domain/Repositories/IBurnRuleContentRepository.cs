using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models.BurnRules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAVN.Service.Campaign.Domain.Repositories
{
    public interface IBurnRuleContentRepository
    {
        Task DeleteAsync(IEnumerable<BurnRuleContentModel> burnRuleContents);

        Task<RuleContentType?> GetContentType(Guid ruleContentId);

        Task<BurnRuleContentModel> GetAsync(Guid contentId);

        Task UpdateAsync(BurnRuleContentModel burnRuleContent);
    }
}
