using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MAVN.Service.Campaign.DomainServices.Services
{
    public class RuleContentValidationService
    : IRuleContentValidationService
    {
        private const string CampaignEarnContentInvalidIdMessage = "Rule does not have any contents with id: {0}";
        private const string DublicatedContentsIds = "The rule contains contents with dublicated ids.";

        public ValidationResult ValidateHaveInvalidOrEmptyIds(IReadOnlyList<Guid> newIds, IReadOnlyList<Guid> oldIds)
        {
            var validationResult = new ValidationResult();

            var editedGuids = newIds.Where(id => id != Guid.Empty);

            if (editedGuids.Count() != editedGuids.Distinct().Count())
            {
                validationResult.Add(DublicatedContentsIds);
            }

            foreach (var newId in newIds)
            {
                // We add check for Id here because Id being Guid Empty is a valid case, it means a new content should be created
                if (newId == Guid.Empty)
                {
                    continue;
                }

                var oldContent = oldIds.FirstOrDefault(c => newId == c);

                if (oldContent == Guid.Empty)
                {
                    validationResult.Add(string.Format(CampaignEarnContentInvalidIdMessage, newId));
                }
            }

            return validationResult;
        }
    }
}
