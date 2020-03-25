using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.BurnRule.Responses;
using Lykke.Service.Campaign.Client.Models.Campaign.Responses;
using Lykke.Service.Campaign.Client.Models.Enums;
using Refit;

namespace Lykke.Service.Campaign.Client.Api
{
    /// <summary>
    /// Provides methods to work with earn and burn rules.
    /// </summary>
    [PublicAPI]
    public interface IHistoryApi
    {
        /// <summary>
        /// Returns earn rules by identifiers.
        /// </summary>
        /// <param name="identifiers">The collection of earn rules identifiers.</param>
        /// <returns>A collection of earn rules.</returns>
        [Get("/api/history/earnRules")]
        Task<IReadOnlyList<CampaignInformationResponseModel>> GetEarnRulesAsync(
            [Query(CollectionFormat.Multi)] Guid[] identifiers);

        /// <summary>
        /// Returns earn rule by identifier.
        /// </summary>
        /// <param name="earnRuleId">The earn rule identifier.</param>
        /// <returns>The earn rule response.</returns>
        [Get("/api/history/earnRules/{earnRuleId}")]
        Task<CampaignDetailResponseModel> GetEarnRuleByIdAsync(Guid earnRuleId);

        /// <summary>
        /// Returns burn rules by identifiers.
        /// </summary>
        /// <param name="identifiers">The collection of burn rules identifiers.</param>
        /// <returns>A collection of burn rules.</returns>
        [Get("/api/history/burnRules")]
        Task<IReadOnlyList<BurnRuleInfoResponse>> GetBurnRulesAsync([Query(CollectionFormat.Multi)] Guid[] identifiers);

        /// <summary>
        /// Returns burn rule by identifier.
        /// </summary>
        /// <param name="burnRuleId">The burn rule identifier.</param>
        /// <returns>The burn rule response.</returns>
        [Get("/api/history/burnRules/{burnRuleId}")]
        Task<BurnRuleResponse> GetBurnRuleByIdAsync(Guid burnRuleId);

        /// <summary>
        /// Returns a localized earn rule model even if it's deleted.
        /// </summary>
        /// <param name="earnRuleId">EarnRule's id</param>
        /// <param name="language">The language of content.</param>
        /// <returns code="200">A earn rule model.</returns>
        [Get("/api/history/earnRulesMobile/{earnRuleId}")]
        Task<EarnRuleLocalizedResponse> GetEarnRuleMobileAsync(Guid earnRuleId, Localization language);
    }
}
