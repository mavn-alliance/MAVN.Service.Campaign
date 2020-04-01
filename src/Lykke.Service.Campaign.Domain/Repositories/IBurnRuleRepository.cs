using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Campaign.Domain.Models.BurnRules;

namespace Lykke.Service.Campaign.Domain.Repositories
{
    public interface IBurnRuleRepository
    {
        Task<Guid> InsertAsync(BurnRuleModel burnRuleModel);

        Task UpdateAsync(BurnRuleModel burnRuleModel);

        Task DeleteAsync(BurnRuleModel burnRuleModel);

        Task<BurnRuleModel> GetAsync(Guid burnRuleId, bool includeDeleted = false);

        Task<PaginatedBurnRuleList> GetPagedAsync(BurnRuleListRequestModel paginationModel, bool includeContents);

        Task<BurnRuleModel> GetByIdAsync(Guid burnRuleId);

        Task<IReadOnlyList<BurnRuleModel>> GetByIdentifiersAsync(IReadOnlyList<Guid> identifiers);
    }
}
