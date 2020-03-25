using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Campaign.Domain.Enums;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Models.EarnRules;

namespace Lykke.Service.Campaign.Domain.Services
{
    public interface ICampaignService
    {
        Task<string> InsertAsync(CampaignDetails campaign);

        Task UpdateAsync(CampaignDetails campaign);

        Task DeleteAsync(string campaignId);

        Task<IReadOnlyCollection<CampaignDetails>> GetCampaignsAsync();

        Task<IReadOnlyCollection<CampaignDetails>> GetActiveCampaignsAsync();

        Task<CampaignDetails> GetCampaignAsync(string campaignId);

        Task<IReadOnlyCollection<CampaignDetails>> GetCampaignsByIdsAsync(Guid[] campaignIds);

        Task<PaginatedCampaignListModel> GetPagedCampaignsAsync(CampaignListRequestModel campaignListRequestModel);

        Task<(bool isSuccessful, string errorMessage)> ProcessOneMinuteTimeEvent(DateTime now);

        Task SaveCampaignContentImage(FileModel file);

        Task<PaginatedEarnRuleListModel> GetEarnRulesPagedAsync(Localization language, List<CampaignStatus> statuses, PaginationModel pagination);

        Task<EarnRuleLocalizedModel> GetAsync(Guid earnRuleId, Localization language);

        Task<CampaignDetails> GetByIdAsync(Guid campaignId);

        Task<EarnRuleLocalizedModel> GetHistoryAsync(Guid earnRuleId, Localization localization);
    }
}
