using FluentValidation;
using Lykke.Service.Campaign.Client.Models.Files.Requests;

namespace Lykke.Service.Campaign.Validation.File
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
