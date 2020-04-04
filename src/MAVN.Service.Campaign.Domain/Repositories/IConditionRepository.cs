using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Campaign.Domain.Models;

namespace MAVN.Service.Campaign.Domain.Repositories
{
    public interface IConditionRepository
    {
        Task<Guid> InsertAsync(Condition condition);
        Task<Condition> GetConditionByIdAsync(Guid conditionId);

        Task<IReadOnlyCollection<Condition>> GetConditionsAsync();

        Task<IReadOnlyCollection<Condition>> GetConditionsByCampaignIdAsync(Guid campaignId);

        Task DeleteAsync(Condition condition);

        Task DeleteAsync(IEnumerable<Condition> conditionIds);

        Task<IReadOnlyCollection<Condition>> GetConditionsForConditionTypeAsync(string conditionType, bool? campaignActive);

        Task RemoveConditionPartnersAsync(Guid conditionId);

        Task DeleteConditionAttributes(Guid conditionId);
    }
}
