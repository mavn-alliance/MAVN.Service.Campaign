using System.Collections.Generic;
using JetBrains.Annotations;

namespace MAVN.Service.Campaign.Client.Models.Campaign.Responses
{
    /// <summary>
    /// Represents entity containing list of Campaigns 
    /// </summary>
    [PublicAPI]
    public class CampaignListResponseModel : CampaignServiceErrorResponseModel
    {
        /// <summary>
        /// List of Campaigns
        /// </summary>
        public IReadOnlyList<CampaignResponse> Campaigns { get; set; } 
    }
}
