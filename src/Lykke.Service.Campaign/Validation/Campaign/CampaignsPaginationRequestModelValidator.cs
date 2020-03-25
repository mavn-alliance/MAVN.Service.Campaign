using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Campaign.Requests;
using Lykke.Service.Campaign.Strings;

namespace Lykke.Service.Campaign.Validation.Campaign
{
    [UsedImplicitly]
    public class CampaignsPaginationRequestModelValidator : AbstractValidator<CampaignsPaginationRequestModel>
    {
        public CampaignsPaginationRequestModelValidator()
        {
            RuleFor(x => x.CurrentPage)
                .LessThanOrEqualTo(10_000)
                .GreaterThanOrEqualTo(1)
                .WithMessage(string.Format(Phrases.PaginationCurrentPageValidation, int.MaxValue))
                .Must((page, list, context) =>
                {
                    context.MessageFormatter.AppendArgument("IntMax", int.MaxValue);
                    context.MessageFormatter.AppendArgument("CurrentPage", nameof(page.CurrentPage));
                    context.MessageFormatter.AppendArgument("PageSize", nameof(page.PageSize));
                    return ((long)page.CurrentPage - 1) * page.PageSize <= int.MaxValue;
                })
                .WithMessage(Phrases.PaginationCurrentMax);
            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage(Phrases.PaginationPageSizeMin)
                .LessThanOrEqualTo(500)
                .WithMessage(Phrases.PaginationPageSizeMax);
        }
    }
}
