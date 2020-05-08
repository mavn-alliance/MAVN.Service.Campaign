using System.Linq;
using MAVN.Numerics.Linq;
using FluentValidation;
using MAVN.Service.Campaign.Client.Models.Condition;

namespace MAVN.Service.Campaign.Validation.Condition
{
    public class RewardRatioAttributeValidator 
        : AbstractValidator<RewardRatioAttribute>
    {
        public RewardRatioAttributeValidator()
        {
            RuleFor(r => r.Ratios)
                .Must(r => r.Sum(rr => rr.PaymentRatio) == 100)
                .WithMessage("The sum of all Payment Ratios should be equal to 100%")
                .Must(r => r.Sum(rr => rr.RewardRatio) == 100)
                .WithMessage("The sum of all Reward Ratios should be equal to 100%")
                .Must(r=> r.GroupBy(x => x.Order).All(x => x.Count() == 1))
                .WithMessage("Orders should be unique");

            RuleForEach(r => r.Ratios)
                .SetValidator(new RatioAttributeValidator());
        }
    }
}
