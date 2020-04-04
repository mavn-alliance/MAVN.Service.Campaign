using System.Collections.Generic;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.Condition;
using MAVN.Service.Campaign.Client.Models.EarnRuleContent;

namespace MAVN.Service.Campaign.Client.Models.Campaign.Requests
{
    /// <inheritdoc />
    /// <summary>
    /// Represent a Campaign Edit model
    /// </summary>
    [PublicAPI]
    public class CampaignEditModel : CampaignBaseModel
    {
        /// <summary>
        /// Represents Identifier of the Campaign
        /// Required field
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Represents Campaign entity conditions
        /// Campaign must have at least one Condition, and must not have more than 1 condition of same type
        /// </summary>
        public IReadOnlyList<ConditionEditModel> Conditions { get; set; }
            = new List<ConditionEditModel>();

        /// <summary>
        ///  Represents Campaign's contents
        /// </summary>
        public IReadOnlyList<EarnRuleContentEditRequest> Contents { get; set; }
             = new List<EarnRuleContentEditRequest>();
    }
}
