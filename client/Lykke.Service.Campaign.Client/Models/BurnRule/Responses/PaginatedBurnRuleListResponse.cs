using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lykke.Service.Campaign.Client.Models.BurnRule.Responses
{
    /// <inheritdoc />
    [PublicAPI]
    public class PaginatedBurnRuleListResponse : BasePaginationResponseModel
    {
        /// <summary>
        /// List of burn rules
        /// </summary>
        public IReadOnlyList<BurnRuleInfoResponse> BurnRules { get; set; }
    }
}
