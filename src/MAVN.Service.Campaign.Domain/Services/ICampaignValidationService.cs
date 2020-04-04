using MAVN.Service.Campaign.Domain.Models;

namespace MAVN.Service.Campaign.Domain.Services
{
    public interface ICampaignValidationService
    {
        ValidationResult ValidateUpdate(CampaignDetails newCampaign, CampaignDetails oldCampaign);
        ValidationResult ValidateInsert(CampaignDetails campaign);
    }
}
