using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.Condition;
using MAVN.Service.Campaign.Strings;

namespace MAVN.Service.Campaign.Validation.Condition
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
