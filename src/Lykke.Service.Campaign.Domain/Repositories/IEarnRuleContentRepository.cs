using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Campaign.Domain.Enums;
using Lykke.Service.Campaign.Domain.Models.EarnRules;

namespace Lykke.Service.Campaign.Domain.Repositories
{
    public interface IEarnRuleContentRepository
    {
        Task DeleteAsync(IEnumerable<EarnRuleContentModel> earnRuleContents);
        Task<RuleContentType?> GetContentType(Guid ruleContentId);
        Task<EarnRuleContentModel> GetAsync(Guid contentId);
        Task UpdateAsync(EarnRuleContentModel earnRuleContent);
    }
}
