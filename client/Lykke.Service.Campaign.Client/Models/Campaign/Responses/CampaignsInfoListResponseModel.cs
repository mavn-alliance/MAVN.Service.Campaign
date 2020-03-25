using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lykke.Service.Campaign.Client.Models.Campaign.Responses
{
    /// <summary>
    /// Represents a model with information for campaigns
    /// </summary>
    [PublicAPI]
    public class CampaignsInfoListResponseModel : CampaignServiceErrorResponseModel
    {
        /// <summary>
        /// List of campaign information model
        /// </summary>
        public IReadOnlyCollection<CampaignInformationResponseModel> Campaigns { get; set; }
    }
}
