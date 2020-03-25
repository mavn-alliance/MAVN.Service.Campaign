using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Api;

namespace Lykke.Service.Campaign.Client
{
    /// <summary>
    /// Campaign service client.
    /// </summary>
    [PublicAPI]
    public interface ICampaignClient
    {
        /// <summary>
        /// Bonus types API.
        /// </summary>
        IBonusTypesApi BonusTypes { get; }

        /// <summary>
        /// Burn rules API.
        /// </summary>
        IBurnRulesApi BurnRules { get; set; }

        /// <summary>
        /// Campaigns API.
        /// </summary>
        ICampaignsApi Campaigns { get; }

        /// <summary>
        /// Conditions API.
        /// </summary>
        IConditionsApi Conditions { get; }

        /// <summary>
        /// History API.
        /// </summary>
        IHistoryApi History { get; set; }

        /// <summary>
        /// Mobile API.
        /// </summary>
        IMobileApi Mobile { get; set; }
    }
}
