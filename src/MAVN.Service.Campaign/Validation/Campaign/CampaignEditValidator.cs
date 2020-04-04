using FluentValidation;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.Campaign.Requests;
using MAVN.Service.Campaign.Strings;
using MAVN.Service.Campaign.Validation.Condition;
using System.Linq;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Validation.EarnRuleContent;

namespace MAVN.Service.Campaign.Validation.Campaign
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
