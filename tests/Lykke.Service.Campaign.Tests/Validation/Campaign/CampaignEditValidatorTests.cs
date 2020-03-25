using FluentValidation.TestHelper;
using Lykke.Service.Campaign.Client.Models.Campaign.Requests;
using Lykke.Service.Campaign.Client.Models.Condition;
using Lykke.Service.Campaign.Validation.Campaign;
using System;
using System.Collections.Generic;
using Lykke.Service.Campaign.Strings;
using Xunit;

namespace Lykke.Service.Campaign.Tests.Validation.Campaign
{
    public class CampaignEditValidatorTests
    {
        private readonly CampaignEditValidator _campaignValidator;

        public CampaignEditValidatorTests()
        {
            _campaignValidator = new CampaignEditValidator();
        }

        [Fact]
        public void When_NoEditConditionPassed_Expect_AnErrorForConditionRequiredThrown()
        {
            var campaign = new CampaignEditModel
            {
                Id = Guid.NewGuid().ToString(),
                FromDate = DateTime.UtcNow.AddMonths(1),
                Conditions = new List<ConditionEditModel>()
            };

            var result = _campaignValidator.ShouldHaveValidationErrorFor(c => c.Conditions, campaign);

            result.WithErrorMessage(Phrases.CampaignConditionNotNull);
        }

        [Fact]
        public void When_TwoEditConditionsOfSameTypePassed_Expect_AnErrorForConditionOfSameTypeThrown()
        {
            const string type = "SignUp";
            var campaign = new CampaignEditModel()
            {
                Id = Guid.NewGuid().ToString(),
                FromDate = DateTime.UtcNow.AddMonths(1),
                Conditions = new List<ConditionEditModel>
                {
                    new ConditionEditModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = type
                    },
                    new ConditionEditModel
                    {
                        Id = Guid.NewGuid().ToString(),
                        Type = type
                    }
                }
            };

            var result = _campaignValidator.ShouldHaveValidationErrorFor(c => c.Conditions, campaign);

            result.WithErrorMessage(Phrases.CampaignConditionUnique);
        }

        [Fact]
        public void When_EarnRuleContentsAreNull_Expect_AnErrorForMissingContentThrown()
        {
            var earnRule = new CampaignEditModel()
            {
                Conditions = null,
                Contents = null
            };

            var contentResult =
                _campaignValidator.ShouldHaveValidationErrorFor(c => c.Contents, earnRule);

            contentResult.WithErrorMessage(Phrases.RuleContentTypeNotNull);

            var conditionResult =
                _campaignValidator.ShouldHaveValidationErrorFor(c => c.Conditions, earnRule);

            conditionResult.WithErrorMessage(Phrases.CampaignConditionNotNull);
        }
    }
}
