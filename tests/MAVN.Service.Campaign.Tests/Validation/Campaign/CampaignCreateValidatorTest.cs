using FluentValidation.TestHelper;
using MAVN.Service.Campaign.Client.Models.Campaign.Requests;
using MAVN.Service.Campaign.Client.Models.Condition;
using MAVN.Service.Campaign.Validation.Campaign;
using System;
using System.Collections.Generic;
using MAVN.Service.Campaign.Strings;
using Xunit;

namespace MAVN.Service.Campaign.Tests.Validation.Campaign
{
    public class CampaignCreateValidatorTest
    {
        private readonly CampaignCreateValidator _campaignCreateValidator;

        public CampaignCreateValidatorTest()
        {
            _campaignCreateValidator = new CampaignCreateValidator();
        }

        [Fact]
        public void When_CampaignFromDateIsInThePast_Expect_AnErrorForCampaignFromDateThrown()
        {
            var campaign = new CampaignCreateModel
            {
                FromDate = DateTime.UtcNow.AddDays(-1)
            };

            var result = _campaignCreateValidator.ShouldHaveValidationErrorFor(x => x.FromDate, campaign);

            result.WithErrorMessage(Phrases.CampaignFromDateValidation);
        }

        [Fact]
        public void When_CampaignToDateIsInThePast_Expect_AnErrorForCampaignFromDateThrown()
        {
            var campaign = new CampaignCreateModel
            {
                ToDate = DateTime.UtcNow.AddDays(-1)
            };

            var result = _campaignCreateValidator.ShouldHaveValidationErrorFor(c => c.ToDate, campaign);

            result.WithErrorMessage(Phrases.CampaignToDateValidation);
        }

        [Fact]
        public void When_NoCreateConditionPassed_Expect_AnErrorForConditionRequiredThrown()
        {
            var campaign = new CampaignCreateModel
            {
                FromDate = DateTime.UtcNow.AddMonths(1),
                Conditions = new List<ConditionCreateModel>()
            };

            var result = _campaignCreateValidator.ShouldHaveValidationErrorFor(c => c.Conditions, campaign);

            result.WithErrorMessage(Phrases.CampaignConditionNotNull);
        }

        [Fact]
        public void When_TwoCreateConditionsOfSameTypePassed_Expect_AnErrorForConditionOfSameTypeThrown()
        {
            const string type = "SignUp";
            var campaign = new CampaignCreateModel
            {
                FromDate = DateTime.UtcNow.AddMonths(1),
                Conditions = new List<ConditionCreateModel>
                {
                    new ConditionCreateModel
                    {
                        Type = type
                    },
                    new ConditionCreateModel
                    {
                        Type = type
                    }
                }
            };

            var result = _campaignCreateValidator.ShouldHaveValidationErrorFor(c => c.Conditions, campaign);

            result.WithErrorMessage(Phrases.CampaignConditionUnique);
        }

        [Fact]
        public void When_EarnRuleContentsAreNull_Expect_AnErrorForMissingContentThrown()
        {
            var earnRule = new CampaignCreateModel()
            {
                Conditions = null,
                Contents = null
            };

            var contentResult =
                _campaignCreateValidator.ShouldHaveValidationErrorFor(c => c.Contents, earnRule);

            contentResult.WithErrorMessage(Phrases.RuleContentTypeNotNull);

            var conditionResult =
                _campaignCreateValidator.ShouldHaveValidationErrorFor(c => c.Conditions, earnRule);

            conditionResult.WithErrorMessage(Phrases.CampaignConditionNotNull);
        }
    }
}
