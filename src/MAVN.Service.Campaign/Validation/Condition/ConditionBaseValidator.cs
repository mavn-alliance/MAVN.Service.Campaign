using FluentValidation;
using MAVN.Service.Campaign.Client.Models.Condition;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Strings;
using System;

namespace MAVN.Service.Campaign.Validation.Condition
{
    public class ConditionBaseValidator<T> : AbstractValidator<T>
        where T : ConditionBaseModel
    {
        public ConditionBaseValidator()
        {
            RuleFor(m => m.CompletionCount)
                .LessThanOrEqualTo(int.MaxValue)
                .Unless(x => x.CompletionCount == null)
                .WithMessage(Phrases.ConditionCompletionMax);

            RuleFor(m => m.CompletionCount)
                .GreaterThan(0)
                .Unless(x => x.CompletionCount == null)
                .WithMessage(Phrases.ConditionCompletionCountValidation);

            RuleFor(c => c.StakeAmount)
                .NotNull()
                .GreaterThan(0)
                .When(c => c.HasStaking);

            RuleFor(c => c.StakeWarningPeriod)
                .GreaterThanOrEqualTo(0)
                .When(c => c.HasStaking);

            RuleFor(c => c.StakingPeriod)
                .NotNull()
                .GreaterThan(0)
                .When(c => c.HasStaking);

            RuleFor(c => c.StakingRule)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .ScalePrecision(2, 5, false)
                .When(c => c.HasStaking);

            RuleFor(c => c.BurningRule)
                .NotNull()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(100)
                .ScalePrecision(2, 5, false)
                .When(c => c.HasStaking);

            RuleFor(m => m.ImmediateReward)
                .NotNull()
                .GreaterThanOrEqualTo(0m)
                .WithMessage(Phrases.ConditionImmediateRewardValidation)
                .Must((model, value) => model.RewardType != RewardType.Percentage || value <= 100)
                .WithMessage("Immediate in percentage should be less than or equal to 100");

            RuleFor(o => o.AmountInTokens)
                .Must((model, value) => model != null && ConversionRateValidator.ValidateConversionRate(model.RewardType,
                                            model.UsePartnerCurrencyRate, () => value.HasValue))
                .WithMessage("Amount in tokens required.")
                .Must((model, value) => model != null && ConversionRateValidator.ValidateConversionRate(model.RewardType,
                                            model.UsePartnerCurrencyRate, () => value.HasValue && value.Value > 0))
                .WithMessage("Amount in tokens should be greater than 0.");

            RuleFor(o => o.AmountInCurrency)
                .Must((model, value) => model != null && ConversionRateValidator.ValidateConversionRate(model.RewardType,
                                            model.UsePartnerCurrencyRate, () => value.HasValue))
                .WithMessage("Amount in currency required.")
                .Must((model, value) => model != null && ConversionRateValidator.ValidateConversionRate(model.RewardType,
                                            model.UsePartnerCurrencyRate, () => value.HasValue && value.Value > 0))
                .WithMessage("Amount in currency should be greater than 0.");

            RuleFor(m => m.ApproximateAward)
                .NotNull()
                .GreaterThanOrEqualTo(0m)
                .WithMessage("Approximate Award is required when Percentage or Conversion Reward Type is selected")
                .When(model => model.RewardType == RewardType.Percentage || model.RewardType == RewardType.ConversionRate);
        }
    }
}
