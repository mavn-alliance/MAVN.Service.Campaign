using System.Collections.Generic;
using MAVN.Service.Campaign.Domain.Models.EarnRules;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class PaginatedEarnRuleListModel : PaginationModel
    {
        public IEnumerable<EarnRuleLocalizedModel> EarnRules { get; set; }
    }
}