using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.BurnRuleContent;

namespace MAVN.Service.Campaign.Client.Models.BurnRule.Requests
{
    /// <summary>
    /// Represents BurnRule's edit request model
    /// </summary>
    [PublicAPI]
    public class BurnRuleEditRequest : BurnRuleBase
    {
        /// <summary>
        /// Represents the identifier of the edited rule
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents BurnRule's contents
        /// </summary>
        public IReadOnlyList<BurnRuleContentEditRequest> BurnRuleContents { get; set; }
            = new List<BurnRuleContentEditRequest>();
    }
}
