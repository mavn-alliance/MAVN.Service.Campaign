using System;
using MAVN.Service.Campaign.Domain.Models;
using System.Collections.Generic;

namespace MAVN.Service.Campaign.Domain.Services
{
    public interface IRuleContentValidationService
    {
        ValidationResult ValidateHaveInvalidOrEmptyIds(IReadOnlyList<Guid> newIds,
            IReadOnlyList<Guid> oldIds);
    }
}
