using System;
using JetBrains.Annotations;

namespace MAVN.Service.Campaign.Client.Models.BurnRule.Responses
{
    /// <summary>
    /// Represents burn rule created response
    /// </summary>
    [PublicAPI]
    public class BurnRuleCreateResponse : CampaignServiceErrorResponseModel
    {
        /// <summary>
        /// Represents created burn rule's identifier
        /// </summary>
        public Guid BurnRuleId { get; set; }
    }
}
