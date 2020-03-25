using Lykke.Service.Campaign.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Campaign.Domain.Enums;

namespace Lykke.Service.Campaign.Domain.Repositories
{
    public interface ICampaignRepository
    {
        Task<Guid> InsertAsync(CampaignDetails campaign);

        Task UpdateAsync(CampaignDetails campaign);

        Task DeleteAsync(CampaignDetails campaign);

        Task<IReadOnlyCollection<CampaignDetails>> GetCampaignsAsync();

        Task<IReadOnlyCollection<CampaignDetails>> GetActiveCampaignsAsync();

        Task<CampaignDetails> GetCampaignAsync(Guid campaignId);

        Task<IReadOnlyCollection<CampaignDetails>> GetCampaignsByIdsAsync(Guid[] campaignIds);

        Task<IReadOnlyCollection<CampaignDetails>> GetAllWithConditionTypeAsync(string conditionType, bool active);

        Task<PaginatedCampaignListModel> GetEnabledCampaignsByStatusAsync(List<CampaignStatus> statuses, PaginationModel pagination);

        Task<PaginatedCampaignListModel> GetPagedCampaignsAsync(CampaignListRequestModel campaignListRequestModel);

        /// <summary>
        /// Returns all campaigns for passed status, including deleted and disabled
        /// </summary>
        /// <param name="status">Campaign status</param>
        /// <param name="date">Date for which the campaign has the passed status</param>
        /// <returns>IReadOnlyList of CampaignDto</returns>
        Task<IReadOnlyList<CampaignDto>> GetAllByStatus(CampaignStatus status, DateTime date);

        Task<CampaignDetails> GetByIdAsync(Guid campaignId);
    }
}
