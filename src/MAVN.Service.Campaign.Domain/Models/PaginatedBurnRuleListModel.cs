using System.Collections.Generic;
using MAVN.Service.Campaign.Domain.Models.BurnRules;

namespace MAVN.Service.Campaign.Domain.Models
{
    public class PaginatedBurnRuleListModel : PaginationModel
    {
        public IEnumerable<BurnRuleLocalizedModel> BurnRules { get; set; }
    }
}
