using System;
using JetBrains.Annotations;

namespace Lykke.Service.Campaign.Client.Models.BurnRuleContent
{
    /// <summary>
    /// Represents burn Rule Content model
    /// </summary>
    [PublicAPI]
    public class BurnRuleContentEditRequest : BurnRuleContentCreateRequest
    {
        /// <summary>
        /// Represents content's identifier
        /// </summary>
        public Guid Id { get; set; }
    }
}
