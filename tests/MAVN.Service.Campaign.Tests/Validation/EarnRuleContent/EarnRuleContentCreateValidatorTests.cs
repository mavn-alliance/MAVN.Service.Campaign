using FluentValidation.TestHelper;
using MAVN.Service.Campaign.Client.Models.EarnRuleContent;
using MAVN.Service.Campaign.Client.Models.Enums;
using MAVN.Service.Campaign.Validation.EarnRuleContent;
using Xunit;

namespace MAVN.Service.Campaign.Tests.Validation.EarnRuleContent
{
    public class EarnRuleContentCreateValidatorTests
    {
        private readonly EarnRuleContentCreateRequestValidator _contentCreateRequestValidator;

        public EarnRuleContentCreateValidatorTests()
        {
            _contentCreateRequestValidator = new EarnRuleContentCreateRequestValidator();
        }

        [Fact]
        public void When_ContentTypeIsTitle_Expect_MaxLengthIs50ErrorIsThrown()
        {
            var content = new EarnRuleContentCreateRequest()
            {
                RuleContentType = RuleContentType.Title,
                Value = new string('a', 100)
            };

            _contentCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.Value, content);
        }

        [Fact]
        public void When_ContentTypeIsDescription_Expect_MaxLengthIs1000ErrorIsThrown()
        {
            var content = new EarnRuleContentCreateRequest()
            {
                RuleContentType = RuleContentType.Description,
                Value = new string('a', 1001)
            };

            _contentCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.Value, content);
        }

        [Fact]
        public void When_ContentTypeIsDescriptionAnd51SymbolsPassed_Expect_MaxLengthErrorIsNtThrown()
        {
            var content = new EarnRuleContentCreateRequest()
            {
                RuleContentType = RuleContentType.Description,
                Value = new string('a', 51)
            };

            _contentCreateRequestValidator.ShouldNotHaveValidationErrorFor(c => c.Value, content);
        }
    }
}
