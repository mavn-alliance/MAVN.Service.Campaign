using FluentValidation;
using Lykke.Service.Campaign.Client.Models.BurnRuleContent;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Strings;

namespace Lykke.Service.Campaign.Validation.BurnRuleContent
{
    public class BurnRuleContentCreateRequestValidator
        : AbstractValidator<BurnRuleContentCreateRequest>
    {
        public BurnRuleContentCreateRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            When(c => c.Localization.Equals(Localization.En), () =>
            {
                RuleFor(c => c.Value)
                    .NotEmpty()
                    .WithMessage(string.Format(Phrases.RequiredValidation, "Title"))
                    .When(c => c.RuleContentType == RuleContentType.Title);

                RuleFor(c => c.Value)
                    .NotEmpty()
                    .WithMessage(string.Format(Phrases.RequiredValidation, "Description"))
                    .When(c => c.RuleContentType == RuleContentType.Description);
            });

            RuleFor(c => c.Value)
                .Must(x => string.IsNullOrEmpty(x) || (x.Length >= 3 && x.Length <= 50))
                .WithMessage(string.Format(Phrases.LengthBetweenValidation, "Title", 3, 50))
                .When(c => c.RuleContentType == RuleContentType.Title);

            RuleFor(c => c.Value)
                .Must(x => string.IsNullOrEmpty(x) || (x.Length >= 3 && x.Length <= 1000))
                .WithMessage(string.Format(Phrases.LengthBetweenValidation, "Description", 3, 1000))
                .When(c => c.RuleContentType == RuleContentType.Description);
        }
    }
}
