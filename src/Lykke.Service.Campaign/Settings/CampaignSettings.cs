using JetBrains.Annotations;

namespace Lykke.Service.Campaign.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CampaignSettings
    {
        public DbSettings Db { get; set; }
        public RabbitMqSettings RabbitMq { get; set; }
        public string BaseCurrencyCode { get; set; }
    }
}
