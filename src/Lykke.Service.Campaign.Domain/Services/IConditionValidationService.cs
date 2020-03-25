using System.Collections.Generic;
using Lykke.Service.Campaign.Domain.Models;

namespace Lykke.Service.Campaign.Domain.Services
{
    public interface IConditionValidationService
    {
        ValidationResult ValidateConditionsAreNotChanged(IReadOnlyList<Condition> newConditions,
            IReadOnlyList<Condition> oldConditions);

        ValidationResult ValidateConditionPropertiesAreNotChanged(Condition newCondition,
            Condition oldCondition);

        ValidationResult ValidateConditionsHaveValidOrEmptyIds(IReadOnlyList<Condition> newConditions,
            IReadOnlyList<Condition> oldConditions);

        ValidationResult ValidateConditionsBonusTypes(IReadOnlyList<Condition> conditions);

        ValidationResult ValidateConditionsPartnersIds(IReadOnlyList<Condition> conditions);
    }
}
