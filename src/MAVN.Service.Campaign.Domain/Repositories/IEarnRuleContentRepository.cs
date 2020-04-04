using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models.EarnRules;

namespace MAVN.Service.Campaign.Domain.Repositories
{
    public interface IEarnRuleContentRepository
    {
        Task DeleteAsync(IEnumerable<EarnRuleContentModel> earnRuleContents);
        Task<RuleContentType?> GetContentType(Guid ruleContentId);
        Task<EarnRuleContentModel> GetAsync(Guid contentId);
        Task UpdateAsync(EarnRuleContentModel earnRuleContent);
    }
}
