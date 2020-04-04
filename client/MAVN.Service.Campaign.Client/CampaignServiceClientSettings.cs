using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.Campaign.Client 
{
    /// <summary>
    /// Campaign client settings.
    /// </summary>
    public class CampaignServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
