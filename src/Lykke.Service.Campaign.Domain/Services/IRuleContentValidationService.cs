using System;
using Lykke.Service.Campaign.Domain.Models;
using System.Collections.Generic;

namespace Lykke.Service.Campaign.Domain.Services
{
    public interface IRuleContentValidationService
    {
        ValidationResult ValidateHaveInvalidOrEmptyIds(IReadOnlyList<Guid> newIds,
            IReadOnlyList<Guid> oldIds);
    }
}
