using FluentValidation;
using MAVN.Service.Campaign.Client.Models.Files.Requests;

namespace MAVN.Service.Campaign.Validation.File
{
    public class FileCreateRequestValidator
        : AbstractValidator<FileCreateRequest>
    {
        public FileCreateRequestValidator()
        {
            RuleFor(c => c.Content)
                .NotEmpty();

            RuleFor(c => c.RuleContentId)
                .NotEmpty();

            RuleFor(c => c.Type)
                .NotEmpty();
        }
    }
}
