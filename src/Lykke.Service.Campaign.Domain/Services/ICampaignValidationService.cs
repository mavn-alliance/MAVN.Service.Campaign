using Lykke.Service.Campaign.Domain.Models;

namespace Lykke.Service.Campaign.Domain.Services
{
    public interface ICampaignValidationService
    {
        ValidationResult ValidateUpdate(CampaignDetails newCampaign, CampaignDetails oldCampaign);
        ValidationResult ValidateInsert(CampaignDetails campaign);
    }
}
