using System.Collections.Generic;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.Condition;
using MAVN.Service.Campaign.Client.Models.EarnRuleContent;

namespace MAVN.Service.Campaign.Client.Models.Campaign.Requests
{
    /// <inheritdoc />
    /// <summary>
    /// Represent a Campaign Create model
    /// </summary>
    [PublicAPI]
    public class CampaignCreateModel : CampaignBaseModel
    {
        /// <summary>
        /// Represents identification of User who created Campaign
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Represents Campaign entity conditions
        /// Campaign must have at least one Condition, and must not have more than 1 condition of same type
        /// </summary>
        public IReadOnlyList<ConditionCreateModel> Conditions { get; set; } 
            = new List<ConditionCreateModel>();

        /// <summary>
        /// Represents Campaign entity's rule contents
        /// </summary>
        public IReadOnlyList<EarnRuleContentCreateRequest> Contents { get; set; }
         = new List<EarnRuleContentCreateRequest>();
    }
}
