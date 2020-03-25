using Lykke.HttpClientGenerator;
using Lykke.Service.Campaign.Client.Api;

namespace Lykke.Service.Campaign.Client
{
    /// <inheritdoc />
    public class CampaignClient : ICampaignClient
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CampaignClient"/> with <param name="httpClientGenerator"></param>.
        /// </summary> 
        public CampaignClient(IHttpClientGenerator httpClientGenerator)
        {
            BonusTypes = httpClientGenerator.Generate<IBonusTypesApi>();
            BurnRules = httpClientGenerator.Generate<IBurnRulesApi>();
            Campaigns = httpClientGenerator.Generate<ICampaignsApi>();
            Conditions = httpClientGenerator.Generate<IConditionsApi>();
            History = httpClientGenerator.Generate<IHistoryApi>();
            Mobile = httpClientGenerator.Generate<IMobileApi>();
        }

        /// <inheritdoc />
        public IBonusTypesApi BonusTypes { get; private set; }

        /// <inheritdoc />
        public IBurnRulesApi BurnRules { get; set; }

        /// <inheritdoc />
        public ICampaignsApi Campaigns { get; private set; }

        /// <inheritdoc />
        public IConditionsApi Conditions { get; private set; }

        /// <inheritdoc />
        public IHistoryApi History { get; set; }

        /// <inheritdoc />
        public IMobileApi Mobile { get; set; }
    }
}
