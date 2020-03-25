using System.Collections.Generic;
using Lykke.Service.Campaign.Client.Models.BurnRule.Requests;
using Lykke.Service.Campaign.Client.Models.BurnRuleContent;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Strings;
using Lykke.Service.Campaign.Validation.BurnRule;
using Xunit;
using FluentValidation.TestHelper;

namespace Lykke.Service.Campaign.Tests.Validation.BurnRule
{
    public class BurnRuleEditRequestValidatorTest
    {
        private readonly BurnRuleEditRequestValidator _burnRuleEditRequestValidator;

        public BurnRuleEditRequestValidatorTest()
        {
            _burnRuleEditRequestValidator = new BurnRuleEditRequestValidator();
        }

        [Fact]
        public void When_BurnRuleDoesNotHaveContents_Expect_AnErrorForMissingContentThrown()
        {
            var burnRule = new BurnRuleEditRequest()
            {
                Title = "title",
                Description = "description"
            };

            var result =
                _burnRuleEditRequestValidator.ShouldHaveValidationErrorFor(c => c.BurnRuleContents, burnRule);

            result.WithErrorMessage(Phrases.RuleContentTypeNotNull);
        }

        [Fact]
        public void When_BurnRuleDoesNotHaveTitleContentInEnglish_Expect_AnErrorForMissingTitleContentThrown()
        {
            var burnRule = new BurnRuleEditRequest()
            {
                Title = "title",
                Description = "description",
                BurnRuleContents = new List<BurnRuleContentEditRequest>()
                {
                    new BurnRuleContentEditRequest()
                    {
                         RuleContentType = RuleContentType.Title,
                         Localization = Localization.Ar,
                         Value = "value"
                    }
                }
            };

            var result =
                _burnRuleEditRequestValidator.ShouldHaveValidationErrorFor(c => c.BurnRuleContents, burnRule);

            result.WithErrorMessage(Phrases.RuleContentTitleNotNull);
        }

        [Fact]
        public void When_BurnRuleHaveTwoTitleContentInEnglish_Expect_AnErrorForUniqueContentThrown()
        {
            var burnRule = new BurnRuleEditRequest()
            {
                Title = "title",
                Description = "description",
                BurnRuleContents = new List<BurnRuleContentEditRequest>()
                {
                    new BurnRuleContentEditRequest()
                    {
                        RuleContentType = RuleContentType.Title,
                        Localization = Localization.En,
                        Value = "value"
                    },
                    new BurnRuleContentEditRequest()
                    {
                        RuleContentType = RuleContentType.Title,
                        Localization = Localization.En,
                        Value = "value1"
                    }
                }
            };

            var result =
                _burnRuleEditRequestValidator.ShouldHaveValidationErrorFor(c => c.BurnRuleContents, burnRule);

            result.WithErrorMessage(Phrases.RuleContentDescriptionNotNull);
        }

        [Fact]
        public void When_BurnRuleTitleNotPassed_Expect_AnErrorForMissingTitleThrown()
        {
            var burnRule = new BurnRuleEditRequest()
            {
                Description = "description",
                BurnRuleContents = new List<BurnRuleContentEditRequest>()
                {
                    new BurnRuleContentEditRequest()
                    {
                        RuleContentType = RuleContentType.Title,
                        Localization = Localization.En,
                        Value = "value"
                    }
                }
            };

            var result =
                _burnRuleEditRequestValidator.ShouldHaveValidationErrorFor(c => c.Title, burnRule);

            result.WithErrorMessage("'Title' must not be empty.");
        }

        [Fact]
        public void When_BurnRuleTitleUnderTwoSymbols_Expect_AnErrorForLengthThrown()
        {
            var burnRule = new BurnRuleEditRequest()
            {
                Title = "ti",
                Description = "description"
            };

            var result =
                _burnRuleEditRequestValidator.ShouldHaveValidationErrorFor(c => c.Title, burnRule);

            result.WithErrorMessage("The length of 'Title' must be at least 3 characters. You entered 2 characters.");
        }

        [Fact]
        public void When_BurnRuleIdNotPassed_Expect_AnErrorIsThrown()
        {
            var burnRule = new BurnRuleEditRequest()
            {
                Title = "ti",
                Description = "description"
            };

            _burnRuleEditRequestValidator.ShouldHaveValidationErrorFor(c => c.Id, burnRule);
        }

        [Fact]
        public void When_BurnRuleContentsAreNull_Expect_AnErrorForMissingContentThrown()
        {
            var burnRule = new BurnRuleEditRequest()
            {
                Title = "title",
                Description = "description",
                BurnRuleContents = null
            };

            var result =
                _burnRuleEditRequestValidator.ShouldHaveValidationErrorFor(c => c.BurnRuleContents, burnRule);

            result.WithErrorMessage(Phrases.RuleContentTypeNotNull);
        }
    }
}
