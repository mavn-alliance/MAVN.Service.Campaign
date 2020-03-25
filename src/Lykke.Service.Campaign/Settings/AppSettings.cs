using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.Campaign.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public CampaignSettings CampaignService { get; set; }
    }
}
