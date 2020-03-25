using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models;
using Lykke.Service.Campaign.Client.Models.BurnRule.Requests;
using Lykke.Service.Campaign.Client.Models.BurnRule.Responses;
using Lykke.Service.Campaign.Client.Models.Files.Requests;
using Refit;

namespace Lykke.Service.Campaign.Client.Api
{
    /// <summary>
    /// BurnRule API
    /// </summary>
    [PublicAPI]
    public interface IBurnRulesApi
    {
        /// <summary>
        /// Returns list of burn rules.
        /// </summary>
        /// <returns>PaginatedBurnRuleListResponse</returns>
        [Get("/api/burn-rules")]
        Task<PaginatedBurnRuleListResponse> GetAsync(BurnRulePaginationRequest burnRulesPaginationRequest);

        /// <summary>
        /// Returns a BurnRule by burnRuleId.
        /// </summary>
        /// <returns>BurnRuleResponse</returns>
        [Get("/api/burn-rules/{burnRuleId}")]
        Task<BurnRuleResponse> GetByIdAsync(Guid burnRuleId);

        /// <summary>
        /// Adds new BurnRule (with content).
        /// </summary>
        /// <param name="model">The model that describes the burn rule</param>
        [Post("/api/burn-rules")]
        Task<BurnRuleCreateResponse> CreateAsync([Body] BurnRuleCreateRequest model);

        /// <summary>
        /// Updates existing BurnRule (with contents).
        /// </summary>
        [Put("/api/burn-rules")]
        Task<BurnRuleResponse> UpdateAsync([Body] BurnRuleEditRequest model);

        /// <summary>
        /// Deletes BurnRule by identification.
        /// </summary>
        [Delete("/api/burn-rules/{burnRuleId}")]
        Task<BurnRuleResponse> DeleteAsync(Guid burnRuleId);

        /// <summary>
        /// Adds new burn rule's content image
        /// </summary>
        /// <param name="model">The model that describes the file.</param>
        [Post("/api/burn-rules/image")]
        Task<CampaignServiceErrorResponseModel> AddImage([Body] FileCreateRequest model);

        /// <summary>
        /// Updates existing burn rule's content image
        /// </summary>
        [Put("/api/burn-rules/image")]
        Task<CampaignServiceErrorResponseModel> UpdateImage([Body] FileEditRequest model);
    }
}
