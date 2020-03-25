using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lykke.Service.Campaign.Client.Models.Campaign.Responses
{
    /// <summary>
    /// Represents a model containing list of Campaigns and pager information
    /// </summary>
    [PublicAPI]
    public class PaginatedCampaignListResponseModel : BasePaginationResponseModel
    {
        /// <summary>
        /// List of Campaigns
        /// </summary>
        public IReadOnlyList<CampaignResponse> Campaigns { get; set; }
    }
}
