using Lykke.Service.Campaign.Domain.Models;

namespace Lykke.Service.Campaign.Domain.Services
{
    public interface IBonusTypeValidationService
    {
        ValidationResult ValidateBonusType(string bonusType, bool validateIfStakeable = false);
    }
}
