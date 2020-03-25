using System.Collections.Generic;
using Lykke.Service.Campaign.Domain.Models.EarnRules;

namespace Lykke.Service.Campaign.Domain.Models
{
    public class PaginatedEarnRuleListModel : PaginationModel
    {
        public IEnumerable<EarnRuleLocalizedModel> EarnRules { get; set; }
    }
}