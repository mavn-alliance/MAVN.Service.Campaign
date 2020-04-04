using MAVN.Service.Campaign.Domain.Models;

namespace MAVN.Service.Campaign.Domain.Services
{
    public interface IBonusTypeValidationService
    {
        ValidationResult ValidateBonusType(string bonusType, bool validateIfStakeable = false);
    }
}
