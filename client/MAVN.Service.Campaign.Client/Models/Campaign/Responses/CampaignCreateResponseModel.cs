using JetBrains.Annotations;

namespace MAVN.Service.Campaign.Client.Models.Campaign.Responses
{
    /// <summary>
    /// Represents result of Campaign creation
    /// </summary>
    [PublicAPI]
    public class CampaignCreateResponseModel : CampaignServiceErrorResponseModel
    {
        /// <summary>
        /// The identifier of the created Campaign
        /// </summary>
        public string CampaignId { get; set; }
    }
}
