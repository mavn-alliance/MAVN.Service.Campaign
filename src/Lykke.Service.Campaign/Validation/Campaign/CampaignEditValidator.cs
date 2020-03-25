using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Campaign.Requests;
using Lykke.Service.Campaign.Strings;
using Lykke.Service.Campaign.Validation.Condition;
using System.Linq;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Validation.EarnRuleContent;

namespace Lykke.Service.Campaign.Validation.Campaign
{
    [UsedImplicitly]
    public class CampaignEditValidator : CampaignBaseValidator<CampaignEditModel>
    {
        public CampaignEditValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty()
                .WithMessage(Phrases.CampaignIdRequired);

            RuleFor(m => m.Conditions)
                .NotEmpty()
                .WithMessage(Phrases.CampaignConditionNotNull)
                .Must(c => c == null || c.GroupBy(x => x.Type).All(x => x.Count() == 1))
                .WithMessage(Phrases.CampaignConditionUnique);

            RuleForEach(m => m.Conditions)
                .SetValidator(new ConditionEditValidator());

            RuleForEach(c => c.Contents)
                .SetValidator(new EarnRuleContentEditRequestValidator());

            RuleFor(b => b.Contents)
                .NotEmpty()
                .WithMessage(Phrases.RuleContentTypeNotNull)
                .Must(c => c == null || c.Any(cc => cc.Localization == Localization.En
                                       && cc.RuleContentType == RuleContentType.Title))
                .WithMessage(Phrases.RuleContentTitleNotNull)
                .Must(c => c == null || c.GroupBy(x => new { x.RuleContentType, x.Localization })
                    .All(x => x.Count() == 1))
                .WithMessage(Phrases.RuleContentUnique);
        }
    }
}
