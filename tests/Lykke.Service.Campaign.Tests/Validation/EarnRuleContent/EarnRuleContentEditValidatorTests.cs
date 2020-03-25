using FluentValidation.TestHelper;
using Lykke.Service.Campaign.Client.Models.EarnRuleContent;
using Lykke.Service.Campaign.Client.Models.Enums;
using Lykke.Service.Campaign.Validation.EarnRuleContent;
using Xunit;

namespace Lykke.Service.Campaign.Tests.Validation.EarnRuleContent
{
    public class EarnRuleContentEditValidatorTests
    {
        private readonly EarnRuleContentEditRequestValidator _contentCreateRequestValidator;

        public EarnRuleContentEditValidatorTests()
        {
            _contentCreateRequestValidator = new EarnRuleContentEditRequestValidator();
        }

        [Fact]
        public void When_ContentTypeIsTitle_Expect_MaxLengthIs50ErrorIsThrown()
        {
            var content = new EarnRuleContentEditRequest()
            {
                RuleContentType = RuleContentType.Title,
                Value = new string('a', 100)
            };

            _contentCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.Value, content);
        }

        [Fact]
        public void When_ContentTypeIsDescription_Expect_MaxLengthIs1000ErrorIsThrown()
        {
            var content = new EarnRuleContentEditRequest()
            {
                RuleContentType = RuleContentType.Description,
                Value = new string('a', 1001)
            };

            _contentCreateRequestValidator.ShouldHaveValidationErrorFor(c => c.Value, content);
        }

        [Fact]
        public void When_ContentTypeIsDescriptionAnd51SymbolsPassed_Expect_MaxLengthErrorIsNtThrown()
        {
            var content = new EarnRuleContentEditRequest()
            {
                RuleContentType = RuleContentType.Description,
                Value = new string('a', 51)
            };

            _contentCreateRequestValidator.ShouldNotHaveValidationErrorFor(c => c.Value, content);
        }
    }
}
