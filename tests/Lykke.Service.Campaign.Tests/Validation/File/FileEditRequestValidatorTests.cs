using FluentValidation.TestHelper;
using Lykke.Service.Campaign.Client.Models.Files.Requests;
using Lykke.Service.Campaign.Validation.File;
using Xunit;

namespace Lykke.Service.Campaign.Tests.Validation.File
{
    public class FileEditRequestValidatorTests
    {
        private readonly FileEditRequestValidator _editRequestValidator;

        public FileEditRequestValidatorTests()
        {
            _editRequestValidator = new FileEditRequestValidator();
        }

        [Fact]
        public void When_FileIdIsEmpty_Expected_AnErrorIsThrown()
        {
            var file = new FileEditRequest() { };

            _editRequestValidator.ShouldHaveValidationErrorFor(c => c.Id, file);
        }

        [Fact]
        public void When_RuleContentIdIsEmpty_Expected_AnErrorIsThrown()
        {
            var file = new FileEditRequest() { };

            _editRequestValidator.ShouldHaveValidationErrorFor(c => c.RuleContentId, file);
        }

        [Fact]
        public void When_ContentEmpty_Expected_AnErrorIsThrown()
        {
            var file = new FileEditRequest() { };

            _editRequestValidator.ShouldHaveValidationErrorFor(c => c.Content, file);
        }

        [Fact]
        public void When_Type_Expected_AnErrorIsThrown()
        {
            var file = new FileEditRequest() { };

            _editRequestValidator.ShouldHaveValidationErrorFor(c => c.Type, file);
        }
    }
}
