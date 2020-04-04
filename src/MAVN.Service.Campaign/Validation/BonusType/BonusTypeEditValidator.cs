using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.BonusType;

namespace MAVN.Service.Campaign.Validation.BonusType
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
