using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.BonusType;

namespace Lykke.Service.Campaign.Validation.BonusType
{
    [UsedImplicitly]
    public class BonusTypeEditValidator : AbstractValidator<BonusTypeEditModel>
    {
        public BonusTypeEditValidator()
        {
            RuleFor(m => m.Type)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(64);

            RuleFor(m => m.DisplayName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(200);
        }
    }
}
