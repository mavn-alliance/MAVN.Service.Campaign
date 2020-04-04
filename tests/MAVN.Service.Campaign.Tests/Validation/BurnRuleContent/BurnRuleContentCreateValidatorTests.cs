using AutoFixture;
using FluentValidation.TestHelper;
using MAVN.Service.Campaign.Client.Models.BurnRuleContent;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Validation.BurnRuleContent;
using Xunit;

namespace MAVN.Service.Campaign.Tests.Validation.BurnRuleContent
{
    public class BurnRuleContentCreateValidatorTests
    {
        private readonly BurnRuleContentCreateRequestValidator _burnRuleContentCreateRequestValidator;
        public BurnRuleContentCreateValidatorTests()
        {
            _burnRuleContentCreateRequestValidator = new BurnRuleContentCreateRequestValidator();
        }

        [Fact]
        public void When_ContentTypeIsTitle_Expect_MaxLengthIs50ErrorIsThrown()
        {
            var content = new BurnRuleContentCreateRequest()
            {
                RuleContentType = RuleContentType.Title,
                Value = new string('a', 100)
            };

            _burnRuleContentCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.Value, content);
        }

        [Fact]
        public void When_ContentTypeIsDescription_Expect_MaxLengthIs1000ErrorIsThrown()
        {
            var content = new BurnRuleContentCreateRequest()
            {
                RuleContentType = RuleContentType.Description,
                Value = new string('a', 1001)
            };

            _burnRuleContentCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.Value, content);
        }

        [Fact]
        public void When_ContentTypeIsDescriptionAnd51SymbolsPassed_Expect_MaxLengthErrorIsNtThrown()
        {
            var content = new BurnRuleContentCreateRequest()
            {
                RuleContentType = RuleContentType.Description,
                Value = new string('a', 51)
            };

            _burnRuleContentCreateRequestValidator.ShouldNotHaveValidationErrorFor(c => c.Value, content);
        }
    }
}
