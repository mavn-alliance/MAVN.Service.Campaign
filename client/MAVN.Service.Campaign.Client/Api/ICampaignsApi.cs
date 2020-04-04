using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models;
using MAVN.Service.Campaign.Client.Models.Campaign.Requests;
using MAVN.Service.Campaign.Client.Models.Campaign.Responses;
using MAVN.Service.Campaign.Client.Models.Files.Requests;
using Refit;

namespace MAVN.Service.Campaign.Client.Api
{
    /// <summary>
    /// Campaign API interface.
    /// </summary>
    [PublicAPI]
    public interface ICampaignsApi
    {
        /// <summary>
        /// Returns list of Campaigns.
        /// </summary>
        /// <returns>CampaignListModel</returns>
        [Get("/api/campaigns")]
        Task<PaginatedCampaignListResponseModel> GetAsync(
            CampaignsPaginationRequestModel campaignsPaginationRequestModel);

        /// <summary>
        /// Returns a Campaign by campaignId.
        /// </summary>
        /// <returns>CampaignResponseModel</returns>
        [Get("/api/campaigns/{campaignId}")]
        Task<CampaignDetailResponseModel> GetByIdAsync(string campaignId);

        /// <summary>
        /// Returns a list of Campaigns by passed campaignIds.
        /// </summary>
        /// <returns>CampaignsInfoListResponseModel</returns>
        [Get("/api/campaigns/all")]
        Task<CampaignsInfoListResponseModel> GetCampaignsByIds([Query(CollectionFormat.Multi)] Guid[] campaignsIds);

        /// <summary>
        /// Adds new Campaign (with conditions).
        /// </summary>
        /// <param name="model">The model that describes instrument.</param>
        [Post("/api/campaigns")]
        Task<CampaignCreateResponseModel> CreateCampaignAsync([Body] CampaignCreateModel model);

        /// <summary>
        /// Updates existing Campaign (with conditions).
        /// </summary>
        [Put("/api/campaigns")]
        Task<CampaignDetailResponseModel> UpdateAsync([Body] CampaignEditModel model);

        /// <summary>
        /// Deletes Campaign by identification.
        /// </summary>
        [Delete("/api/campaigns/{campaignId}")]
        Task<CampaignDetailResponseModel> DeleteAsync(string campaignId);

        /// <summary>
        /// Adds new Campaign's content image
        /// </summary>
        /// <param name="model">The model that describes the file.</param>
        [Post("/api/campaigns/image")]
        Task<CampaignServiceErrorResponseModel> AddImage([Body] FileCreateRequest model);

        /// <summary>
        /// Updates existing Campaign's content image
        /// </summary>
        [Put("/api/campaigns/image")]
        Task<CampaignServiceErrorResponseModel> UpdateImage([Body] FileEditRequest model);
    }
}
