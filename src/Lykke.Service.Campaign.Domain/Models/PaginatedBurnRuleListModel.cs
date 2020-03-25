using System.Collections.Generic;
using Lykke.Service.Campaign.Domain.Models.BurnRules;

namespace Lykke.Service.Campaign.Domain.Models
{
    public class PaginatedBurnRuleListModel : PaginationModel
    {
        public IEnumerable<BurnRuleLocalizedModel> BurnRules { get; set; }
    }
}
