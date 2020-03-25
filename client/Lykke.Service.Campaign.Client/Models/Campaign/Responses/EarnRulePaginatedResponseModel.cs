using System.Collections.Generic;

namespace Lykke.Service.Campaign.Client.Models.Campaign.Responses
{
    /// <summary>
    /// Paginated response for Earn Rules
    /// </summary>
    public class EarnRulePaginatedResponseModel : BasePaginationResponseModel
    {
        /// <summary>
        /// Earn rules
        /// </summary>
        public IReadOnlyList<EarnRuleLocalizedResponse> EarnRules { get; set; }
    }
}
