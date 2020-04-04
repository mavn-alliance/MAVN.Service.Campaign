using System.Linq;
using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.BurnRule;

namespace MAVN.Service.Campaign.Validation.BurnRule
{
    [UsedImplicitly]
    public class BurnRuleBaseValidator<T> : AbstractValidator<T>
        where T : BurnRuleBase
    {
        public BurnRuleBaseValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(b => b.Title)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(50);

            RuleFor(m => m.Description)
                .MinimumLength(3)
                .MaximumLength(1000);

            RuleFor(o => o.AmountInTokens)
                .Must((model, value) => model != null && (model.UsePartnerCurrencyRate ||
                                                          !model.UsePartnerCurrencyRate && value.HasValue))
                .WithMessage("Amount in tokens required.")
                .Must((model, value) => model != null && (model.UsePartnerCurrencyRate ||
                                                          !model.UsePartnerCurrencyRate &&
                                                          value.HasValue && value.Value > 0))
                .WithMessage("Amount in tokens should be greater than 0.");

            RuleFor(o => o.AmountInCurrency)
                .Must((model, value) => model != null && (model.UsePartnerCurrencyRate ||
                                                          !model.UsePartnerCurrencyRate && value.HasValue))
                .WithMessage("Amount in currency required.")
                .Must((model, value) => model != null && (model.UsePartnerCurrencyRate ||
                                                          !model.UsePartnerCurrencyRate &&
                                                          value.HasValue && value.Value > 0))
                .WithMessage("Amount in currency should be greater than 0.");

            RuleFor(o => o.Vertical)
                .NotNull()
                .NotEmpty()
                .WithMessage("Vertical is required.");

            RuleFor(o => o.PartnerIds)
                .Must(p => p == null || p.GroupBy(n => n).All(c => c.Count() == 1))
                .WithMessage("Partner IDs must not have duplicates.");

            RuleFor(m => m.Order)
                .InclusiveBetween(0, int.MaxValue);
        }
    }
}
