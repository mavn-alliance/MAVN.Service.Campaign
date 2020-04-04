using System.Collections.Generic;

namespace MAVN.Service.Campaign.Domain.Models.BurnRules
{
    public class PaginatedBurnRuleList : PaginationModel
    {
        public IReadOnlyList<BurnRuleModel> BurnRules { get; set; }
    }
}
