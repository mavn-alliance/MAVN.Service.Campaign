using System.Collections.Generic;
using FluentValidation.TestHelper;
using MAVN.Service.Campaign.Client.Models.BurnRule.Requests;
using MAVN.Service.Campaign.Client.Models.BurnRuleContent;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Strings;
using MAVN.Service.Campaign.Validation.BurnRule;
using Xunit;

namespace MAVN.Service.Campaign.Tests.Validation.BurnRule
{
    public class BurnRuleCreateRequestValidatorTests
    {
        private readonly BurnRuleCreateRequestValidator _burnRuleCreateRequestValidator;

        public BurnRuleCreateRequestValidatorTests()
        {
            _burnRuleCreateRequestValidator = new BurnRuleCreateRequestValidator();
        }

        [Fact]
        public void When_BurnRuleDoesNotHaveContents_Expect_AnErrorForMissingContentThrown()
        {
            var burnRule = new  BurnRuleCreateRequest()
            {
                Title = "title",
                Description = "description"
            };

            var result =
                _burnRuleCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.BurnRuleContents, burnRule);

            result.WithErrorMessage(Phrases.RuleContentTypeNotNull);
        }

        [Fact]
        public void When_BurnRuleDoesNotHaveTitleContentInEnglish_Expect_AnErrorForMissingTitleContentThrown()
        {
            var burnRule = new BurnRuleCreateRequest()
            {
                Title = "title",
                Description = "description",
                BurnRuleContents = new List<BurnRuleContentCreateRequest>()
                {
                    new BurnRuleContentCreateRequest()
                    {
                         RuleContentType = RuleContentType.Title,
                         Localization = Localization.Ar,
                         Value = "value"
                    }
                }
            };

            var result =
                _burnRuleCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.BurnRuleContents, burnRule);

            result.WithErrorMessage(Phrases.RuleContentTitleNotNull);
        }

        [Fact]
        public void When_BurnRuleHaveTwoTitleContentInEnglish_Expect_AnErrorForUniqueContentThrown()
        {
            var burnRule = new BurnRuleCreateRequest()
            {
                Title = "title",
                Description = "description",
                BurnRuleContents = new List<BurnRuleContentCreateRequest>()
                {
                    new BurnRuleContentCreateRequest()
                    {
                        RuleContentType = RuleContentType.Title,
                        Localization = Localization.En,
                        Value = "value"
                    },
                    new BurnRuleContentCreateRequest()
                    {
                        RuleContentType = RuleContentType.Title,
                        Localization = Localization.En,
                        Value = "value1"
                    }
                }
            };

            var result =
                _burnRuleCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.BurnRuleContents, burnRule);

            result.WithErrorMessage(Phrases.RuleContentDescriptionNotNull);
        }

        [Fact]
        public void When_BurnRuleTitleNotPassed_Expect_AnErrorForMissingTitleThrown()
        {
            var burnRule = new BurnRuleCreateRequest()
            {
                Description = "description",
                BurnRuleContents = new List<BurnRuleContentCreateRequest>()
                {
                    new BurnRuleContentCreateRequest()
                    {
                        RuleContentType = RuleContentType.Title,
                        Localization = Localization.En,
                        Value = "value"
                    }
                }
            };

            var result =
                _burnRuleCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.Title, burnRule);

            result.WithErrorMessage("'Title' must not be empty.");
        }

        [Fact]
        public void When_BurnRuleTitleUnderTwoSymbols_Expect_AnErrorForLengthThrown()
        {
            var burnRule = new BurnRuleCreateRequest()
            {
                Title = "ti",
                Description = "description"
            };

            var result =
                _burnRuleCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.Title, burnRule);

            result.WithErrorMessage("The length of 'Title' must be at least 3 characters. You entered 2 characters.");
        }

        [Fact]
        public void When_BurnRuleContentsAreNull_Expect_AnErrorForMissingContentThrown()
        {
            var burnRule = new BurnRuleCreateRequest()
            {
                Title = "title",
                Description = "description",
                BurnRuleContents = null
            };

            var result =
                _burnRuleCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.BurnRuleContents, burnRule);

            result.WithErrorMessage(Phrases.RuleContentTypeNotNull);
        }
    }
}
