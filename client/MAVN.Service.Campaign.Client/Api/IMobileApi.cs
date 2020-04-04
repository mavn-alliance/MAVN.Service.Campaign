using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models;
using MAVN.Service.Campaign.Client.Models.BurnRule.Responses;
using MAVN.Service.Campaign.Client.Models.Campaign.Responses;
using MAVN.Service.Campaign.Client.Models.Enums;
using Refit;

namespace MAVN.Service.Campaign.Client.Api
{
    /// <summary>
    /// Provides methods to work with mobile data.
    /// </summary>
    [PublicAPI]
    public interface IMobileApi
    {
        /// <summary>
        /// Returns a collection of localized spend rules.
        /// </summary>
        /// <param name="language">The language of content.</param>
        /// <param name="pagination">Pagination</param>
        /// <returns>A collection of spend rules.</returns>
        [Get("/api/mobile/burn-rules")]
        Task<BurnRulePaginatedResponseModel> GetSpendRulesAsync(Localization language, BasePaginationRequestModel pagination);

        /// <summary>
        /// Returns a localized spend rule.
        /// </summary>
        /// <param name="burnRuleId">SpendRule's id</param>
        /// <param name="language">The language of content.</param>
        /// <param name="includeDeleted">Indicates that the deleted spend rules should be returned.</param>
        /// <returns>A spend rule.</returns>
        [Get("/api/mobile/burn-rules/{burnRuleId}")]
        Task<BurnRuleLocalizedResponse> GetSpendRuleAsync(Guid burnRuleId, Localization language, bool includeDeleted = false);

        /// <summary>
        /// Returns a collection of localized earn rules.
        /// </summary>
        /// <param name="language">The language of content.</param>
        /// <param name="statuses">Filter by earn rule's status</param>
        /// <param name="pagination">Pagination</param>
        /// <returns>A collection of earn rules.</returns>
        [Get("/api/mobile/earn-rules")]
        Task<EarnRulePaginatedResponseModel> GetEarnRulesAsync(Localization language,
            [Query(CollectionFormat.Multi)] CampaignStatus[] statuses, BasePaginationRequestModel pagination);

        /// <summary>
        /// Returns a localized earn rule.
        /// </summary>
        /// <param name="earnRuleId">Earn rule's id</param>
        /// <param name="language">The language of content.</param>
        /// <returns>An earn rule model</returns>
        [Get("/api/mobile/earn-rules/{earnRuleId}")]
        Task<EarnRuleLocalizedResponse> GetEarnRuleAsync(Guid earnRuleId, Localization language);
    }
}
