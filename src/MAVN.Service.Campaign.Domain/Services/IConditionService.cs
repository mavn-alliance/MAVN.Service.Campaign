using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Campaign.Domain.Models;

namespace MAVN.Service.Campaign.Domain.Services
{
    public interface IConditionService
    {
        Task<string> InsertAsync(Condition condition);

        Task<IReadOnlyCollection<Condition>> GetConditionsAsync();

        Task<IReadOnlyCollection<Condition>> GetConditionsByCampaignIdAsync(string campaignId);
        
        Task<Condition> GetConditionByIdAsync(Guid conditionId);

        Task<IReadOnlyCollection<Condition>> GetConditionsForConditionTypeAsync(string conditionType, bool? campaignActive = null);

        Task DeleteAsync(IEnumerable<Condition> conditionsToRemove);

        Task DeleteConditionPartnersAsync(IEnumerable<Guid> conditionsIds);

        Task DeleteConditionAttributesAsync(IEnumerable<Guid> conditionsId);
    }
}
