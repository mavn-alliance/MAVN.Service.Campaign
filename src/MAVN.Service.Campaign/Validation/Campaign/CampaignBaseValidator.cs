using System;
using FluentValidation;
using MAVN.Service.Campaign.Client.Models.Campaign.Requests;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Strings;

namespace MAVN.Service.Campaign.Validation.Campaign
{
    public class CampaignBaseValidator<T> : AbstractValidator<T>
        where T : CampaignBaseModel
    {
        public CampaignBaseValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(m => m.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(m => m.Description)
                .MinimumLength(3)
                .MaximumLength(1000);

            RuleFor(m => m.Reward)
                .NotNull()
                .GreaterThanOrEqualTo(0m)
                .WithMessage(Phrases.CampaignRewardGreaterOrEqualToZero)
                .Must((model, value) => model.RewardType != RewardType.Percentage || value <= 100)
                .WithMessage("Reward in percentage should be less than or equal to 100");

            RuleFor(m => m.ApproximateAward)
                .NotNull()
                .GreaterThanOrEqualTo(0m)
                .WithMessage("Approximate Award is required when Percentage or Conversion Reward Type is selected")
                .When(model => model.RewardType == RewardType.Percentage || model.RewardType == RewardType.ConversionRate);

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

            RuleFor(m => m.FromDate)
                .NotEmpty()
                .WithMessage(Phrases.CampaignFromDateRequired);

            RuleFor(m => m.ToDate)
                .GreaterThan(m => m.FromDate)
                .WithMessage(Phrases.CampaignToBeforeFromDateValidation);

            RuleFor(m => m.CompletionCount)
                .Must(value => !value.HasValue || value > 0)
                .WithMessage(Phrases.CampaignCompletionMin);

            RuleFor(m => m.Order)
                .InclusiveBetween(0, int.MaxValue);
        }
    }
}
