using FluentValidation.TestHelper;
using MAVN.Service.Campaign.Client.Models.Campaign.Requests;
using MAVN.Service.Campaign.Validation.Campaign;
using System;
using Xunit;

namespace MAVN.Service.Campaign.Tests.Validation.Campaign
{
    public class CampaignBaseValidatorTests
    {
        private readonly CampaignBaseValidator<CampaignBaseModel> _campaignValidator;

        public CampaignBaseValidatorTests()
        {
            _campaignValidator = new CampaignBaseValidator<CampaignBaseModel>();
        }

        [Fact]
        public void When_CampaignFromDateIsInPresent_Expect_NoErrorsForCampaignFromDateAreThrown()
        {
            var campaign = new CampaignBaseModel
            {
                FromDate = DateTime.UtcNow
            };

            _campaignValidator.ShouldNotHaveValidationErrorFor(c => c.FromDate, campaign);
        }

        [Fact]
        public void When_CampaignFromDateIsInFuture_Expect_NoErrorsForCampaignFromDateAreThrown()
        {
            var campaign = new CampaignBaseModel
            {
                FromDate = DateTime.UtcNow.AddMinutes(1)
            };

            _campaignValidator.ShouldNotHaveValidationErrorFor(c => c.FromDate, campaign);
        }

        [Fact]
        public void When_CampaignToDateIsInPresent_Expect_NoErrorsForCampaignFromDateAreThrown()
        {
            var campaign = new CampaignBaseModel
            {
                ToDate = DateTime.UtcNow
            };

            _campaignValidator.ShouldNotHaveValidationErrorFor(c => c.ToDate, campaign);
        }

        [Fact]
        public void When_CampaignToDateIsInFuture_Expect_NoErrorsForCampaignFromDateAreThrown()
        {
            var campaign = new CampaignBaseModel
            {
                ToDate = DateTime.UtcNow.AddMinutes(1)
            };

            _campaignValidator.ShouldNotHaveValidationErrorFor(c => c.ToDate, campaign);
        }

        [Fact]
        public void When_CampaignRewardIsWholeNumberAndRewardTypeIsFixed_Expect_NoErrorsForRewardAreThrown()
        {
            var campaign = new CampaignBaseModel
            {
                ToDate = DateTime.UtcNow,
                Reward = 1.0m
            };

            _campaignValidator.ShouldNotHaveValidationErrorFor(c => c.Reward, campaign);
        }

        [Fact]
        public void When_CampaignRewardIsZeroAndRewardTypeIsFixed_Expect_NoErrorsForRewardAreThrown()
        {
            var campaign = new CampaignBaseModel
            {
                ToDate = DateTime.UtcNow,
                Reward = 0
            };

            _campaignValidator.ShouldNotHaveValidationErrorFor(c => c.Reward, campaign);
        }

        [Fact]
        public void When_CampaignRewardIsNull_Expect_ErrorsForRewardAreThrown()
        {
            var campaign = new CampaignBaseModel
            {
                ToDate = DateTime.UtcNow
            };

            _campaignValidator.ShouldHaveValidationErrorFor(c => c.Reward, campaign);
        }

        [Fact]
        public void When_CampaignRewardIsExplicitNull_Expect_ErrorsForRewardAreThrown()
        {
            var campaign = new CampaignBaseModel
            {
                ToDate = DateTime.UtcNow,
                Reward = null
            };

            _campaignValidator.ShouldHaveValidationErrorFor(c => c.Reward, campaign);
        }
    }
}
