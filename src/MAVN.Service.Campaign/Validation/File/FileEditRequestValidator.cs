using FluentValidation;
using MAVN.Service.Campaign.Client.Models.Files.Requests;

namespace MAVN.Service.Campaign.Validation.File
{
    public class FileEditRequestValidator
        : AbstractValidator<FileEditRequest>
    {
        public FileEditRequestValidator()
        {
            RuleFor(c => c.Id)
                .NotEmpty();

            RuleFor(c => c.Content)
                .NotEmpty();

            RuleFor(c => c.RuleContentId)
                .NotEmpty();

            RuleFor(c => c.Type)
                .NotEmpty();
        }
    }
}
