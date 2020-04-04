using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Services;

namespace MAVN.Service.Campaign.DomainServices.Services
{
    public class BonusTypeValidationService : IBonusTypeValidationService
    {
        private readonly IBonusTypeService _bonusTypeService;

        public BonusTypeValidationService(IBonusTypeService bonusTypeService)
        {
            _bonusTypeService = bonusTypeService;
        }
        public ValidationResult ValidateBonusType(string bonusType, bool validateIfStakeable = false)
        {
            var validationResult = new ValidationResult();
            var type = _bonusTypeService.GetAsync(bonusType).GetAwaiter().GetResult();

            if (type == null)
            {
                validationResult.Add($"Condition Type {bonusType} is not a valid Type");
                return validationResult;
            }

            if (!type.IsAvailable)
            {
                validationResult.Add($"Condition Type {bonusType} is not available Type");
            }

            if (validateIfStakeable && !type.IsStakeable)
            {
                validationResult.Add($"Condition Type {bonusType} is not stakeable Type");
            }

            return validationResult;
        }
    }
}
