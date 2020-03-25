using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Condition;
using Lykke.Service.Campaign.Strings;

namespace Lykke.Service.Campaign.Validation.Condition
{
    [UsedImplicitly]
    public class ConditionValidator : ConditionBaseValidator<ConditionModel>
    {
        public ConditionValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty()
                .WithMessage(Phrases.ConditionIdRequired);
        }
    }
}
