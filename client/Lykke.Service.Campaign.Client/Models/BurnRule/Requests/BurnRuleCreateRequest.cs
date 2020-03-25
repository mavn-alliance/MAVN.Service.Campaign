using System.Collections.Generic;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.BurnRuleContent;

namespace Lykke.Service.Campaign.Client.Models.BurnRule.Requests
{
    /// <summary>
    /// Represents Burn rule create request model
    /// </summary>
    [PublicAPI]
    public class BurnRuleCreateRequest : BurnRuleBase
    {
        /// <summary>
        /// Represents identification of User who created the earn rule
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents BurnRule's entity contents
        /// </summary>
        public IReadOnlyList<BurnRuleContentCreateRequest> BurnRuleContents { get; set; }
            = new List<BurnRuleContentCreateRequest>();
    }
}
