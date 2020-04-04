using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Campaign.Domain.Enums;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Models.BurnRules;

namespace MAVN.Service.Campaign.Domain.Services
{
    public interface IBurnRuleService
    {
        Task<Guid> InsertAsync(BurnRuleModel burnRuleModel);

        Task UpdateAsync(BurnRuleModel burnRuleModel);

        Task DeleteAsync(Guid burnRuleId);

        Task<BurnRuleModel> GetAsync(Guid burnRuleId);
        
        Task<IReadOnlyList<BurnRuleModel>> GetAsync(IReadOnlyList<Guid> identifiers);

        Task<PaginatedBurnRuleList> GetPagedAsync(BurnRuleListRequestModel paginationModel);

        Task<PaginatedBurnRuleListModel> GetLocalizedPagedAsync(Localization language, PaginationModel paginationModel);

        Task<BurnRuleLocalizedModel> GetAsync(Guid id, Localization language, bool includeDeleted = false);

        Task SaveBurnRuleContentImage(FileModel file);

        Task<BurnRuleModel> GetByIdAsync(Guid burnRuleId);
    }
}
