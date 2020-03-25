using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Condition;

namespace Lykke.Service.Campaign.Validation.Condition
{
    [UsedImplicitly]
    public class ConditionEditValidator : ConditionBaseValidator<ConditionEditModel>
    {
        public ConditionEditValidator()
        {
            RuleFor(c => c.RewardRatio)
                .SetValidator(new RewardRatioAttributeValidator())
                .When(c => c.RewardHasRatio);

            RuleFor(c => c.RewardRatio)
                .NotNull()
                .When(c => c.RewardHasRatio);
        }
    }
}
