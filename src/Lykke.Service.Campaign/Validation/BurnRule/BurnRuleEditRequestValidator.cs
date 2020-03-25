using System.Linq;
using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.BurnRule.Requests;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Strings;
using Lykke.Service.Campaign.Validation.BurnRuleContent;

namespace Lykke.Service.Campaign.Validation.BurnRule
{
    [UsedImplicitly]
    public class BurnRuleEditRequestValidator :
        BurnRuleBaseValidator<BurnRuleEditRequest>
    {
        public BurnRuleEditRequestValidator()
        {
            RuleFor(b => b.Id)
                .NotEmpty();

            RuleFor(b => b.BurnRuleContents)
                .NotEmpty()
                .WithMessage(Phrases.RuleContentTypeNotNull)
                .Must(c => c == null || c.Any(cc => cc.Localization == Localization.En
                                       && cc.RuleContentType == RuleContentType.Title))
                .WithMessage(Phrases.RuleContentTitleNotNull)
                .Must(c => c == null || c.Any(cc => cc.Localization == Localization.En
                                                    && cc.RuleContentType == RuleContentType.Description))
                .WithMessage(Phrases.RuleContentDescriptionNotNull)
                .Must(c => c == null || c.GroupBy(x => new { x.RuleContentType, x.Localization })
                    .All(x => x.Count() == 1))
                .WithMessage(Phrases.RuleContentUnique);

            RuleForEach(b => b.BurnRuleContents)
                .SetValidator(new BurnRuleContentCreateRequestValidator());
        }
    }
}
