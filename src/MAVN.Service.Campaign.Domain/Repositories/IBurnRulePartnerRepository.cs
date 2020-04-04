using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAVN.Service.Campaign.Domain.Repositories
{
    public interface IBurnRulePartnerRepository
    {
        Task DeleteAsync(IEnumerable<Guid> partners, Guid burnRuleId);
        Task InsertAsync(IEnumerable<Guid> partners, Guid burnRuleId);
    }
}
