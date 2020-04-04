using System.Collections.Generic;

namespace MAVN.Service.Campaign.Client.Models.BurnRule.Responses
{
    /// <summary>
    /// Paginated response for Burn Rules
    /// </summary>
    public class BurnRulePaginatedResponseModel : BasePaginationResponseModel
    {
        /// <summary>
        /// Burn rules
        /// </summary>
        public IReadOnlyList<BurnRuleLocalizedResponse> BurnRules { get; set; }
    }
}
