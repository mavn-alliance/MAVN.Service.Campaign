using System;
using MAVN.Service.Campaign.Domain.Extensions;
using MAVN.Service.Campaign.Domain.Models;
using MAVN.Service.Campaign.Domain.Repositories;
using MAVN.Service.Campaign.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Campaign.Domain.Exceptions;

namespace MAVN.Service.Campaign.DomainServices.Services
{
    public class ConditionService : IConditionService
    {
        private readonly IConditionRepository _conditionRepository;

        public ConditionService(IConditionRepository conditionRepository)
        {
            _conditionRepository = conditionRepository;
        }

        public async Task<string> InsertAsync(Condition condition)
        {
            var conditionId = await _conditionRepository.InsertAsync(condition);
            return conditionId.ToString("D");
        }

        public async Task<IReadOnlyCollection<Condition>> GetConditionsAsync()
        {
            return await _conditionRepository.GetConditionsAsync();
        }

        public async Task<IReadOnlyCollection<Condition>> GetConditionsByCampaignIdAsync(string campaignId)
        {
            return await _conditionRepository.GetConditionsByCampaignIdAsync(campaignId.ToGuid());
        }

        public async Task<Condition> GetConditionByIdAsync(Guid conditionId)
        {
            var condition = await _conditionRepository.GetConditionByIdAsync(conditionId);

            if (condition == null)
            {
                throw new EntityNotFoundException($"Condition with id {conditionId} does not exist.");
            }

            return condition;
        }

        public async Task<IReadOnlyCollection<Condition>> GetConditionsForConditionTypeAsync(string conditionType, bool? campaignActive = null)
        {
            return await _conditionRepository.GetConditionsForConditionTypeAsync(conditionType, campaignActive);
        }

        public async Task DeleteAsync(IEnumerable<Condition> conditionsToRemove)
        {
            await _conditionRepository.DeleteAsync(conditionsToRemove);
        }

        public async Task DeleteConditionPartnersAsync(IEnumerable<Guid> conditionsId)
        {
            foreach (var condition in conditionsId)
            {
                await _conditionRepository.RemoveConditionPartnersAsync(condition);
            }
        }

        public async Task DeleteConditionAttributesAsync(IEnumerable<Guid> conditionsId)
        {
            foreach (var condition in conditionsId)
            {
                await _conditionRepository.DeleteConditionAttributes(condition);
            }
        }
    }
}
