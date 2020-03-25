using System.Linq;
using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Condition;

namespace Lykke.Service.Campaign.Validation.Condition
{
    [UsedImplicitly]
    public class ConditionCreateValidator : ConditionBaseValidator<ConditionCreateModel>
    {
        public ConditionCreateValidator()
        {
            RuleFor(c => c.PartnerIds)
                .Must(p => p == null || p.Distinct().Count() == p.Length)
                .WithMessage("You can not assign more than once one partner to the same condition");

            RuleFor(c => c.RewardRatio)
                .NotNull()
                .When(c => c.RewardHasRatio);

            RuleFor(c => c.RewardRatio)
                .SetValidator(new RewardRatioAttributeValidator())
                .When(c => c.RewardHasRatio);
        }
    }
}
