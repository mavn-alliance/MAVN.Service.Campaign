using System;
using System.Linq;
using FluentValidation;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Campaign.Requests;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Strings;
using Lykke.Service.Campaign.Validation.Condition;
using Lykke.Service.Campaign.Validation.EarnRuleContent;

namespace Lykke.Service.Campaign.Validation.Campaign
{
    [UsedImplicitly]
    public class CampaignCreateValidator : CampaignBaseValidator<CampaignCreateModel>
    {
        public CampaignCreateValidator()
        {
            // Changed to 00:00:00:000 because FE don't have date and time control yet. 
            var currentTime = DateTime.UtcNow.Date;

            RuleFor(m => m.Conditions)
                .NotEmpty()
                .WithMessage(Phrases.CampaignConditionNotNull)
                .Must(c => c == null || c.GroupBy(x => x.Type).All(x => x.Count() == 1))
                .WithMessage(Phrases.CampaignConditionUnique);

            RuleFor(m => m.FromDate)
                .GreaterThanOrEqualTo(currentTime)
                .WithMessage(Phrases.CampaignFromDateValidation);

            RuleFor(m => m.ToDate)
                .GreaterThanOrEqualTo(currentTime)
                .WithMessage(Phrases.CampaignToDateValidation);

            RuleForEach(m => m.Conditions)
                .SetValidator(new ConditionCreateValidator());

            RuleForEach(c => c.Contents)
                .SetValidator(new EarnRuleContentCreateRequestValidator());

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
