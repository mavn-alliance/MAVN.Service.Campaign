using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.BurnRuleContent;
using MAVN.Service.Campaign.Client.Models.Enums;

namespace MAVN.Service.Campaign.Client.Models.BurnRule.Responses
{
    /// <summary>
    /// Represents burn Rule Response
    /// </summary>
    [PublicAPI]
    public class BurnRuleResponse : BurnRuleBase
    {
        /// <summary>
        /// Represents burn Rule identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents error code from the operation.
        /// </summary>
        public CampaignServiceErrorCodes ErrorCode { get; set; }

        /// <summary>
        /// Represents error message from the operation.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Represents burnRule's entity contents
        /// </summary>
        public IReadOnlyList<BurnRuleContentResponse> BurnRuleContents { get; set; }
            = new List<BurnRuleContentResponse>();
    }
}
