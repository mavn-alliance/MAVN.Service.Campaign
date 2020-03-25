using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Campaign.Domain.Exceptions;
using Lykke.Service.Campaign.Domain.Models;
using Lykke.Service.Campaign.Domain.Repositories;
using Lykke.Service.Campaign.Domain.Services;

namespace Lykke.Service.Campaign.DomainServices.Services
{
    public class BonusTypeService : IBonusTypeService
    {
        private readonly IBonusTypeRepository _bonusTypeRepository;
        private readonly ILog _log;

        public BonusTypeService(IBonusTypeRepository conditionTypeRepository,
            ILogFactory logFactory)
        {
            _bonusTypeRepository = conditionTypeRepository;
            _log = logFactory.CreateLog(this);
        }

        public async Task<IReadOnlyCollection<BonusType>> GetBonusTypesAsync()
        {
            return await _bonusTypeRepository.GetBonusTypesAsync();
        }

        public async Task<BonusType> GetAsync(string bonusTypeName)
        {
            return await _bonusTypeRepository.GetBonusTypeAsync(bonusTypeName);
        }

        public async Task<IReadOnlyCollection<BonusType>> GetActiveBonusTypesAsync()
        {
            return await _bonusTypeRepository.GetActiveBonusTypesAsync();
        }

        public async Task<BonusType> InsertAsync(BonusType bonusType)
        {
            bonusType.CreationDate = DateTime.UtcNow;
            bonusType.Type = bonusType.Type.ToLower();

            var bonusTypeWithSameType = await _bonusTypeRepository.GetBonusTypeAsync(bonusType.Type);

            if (bonusTypeWithSameType != null)
            {
                throw new EntityAlreadyExistsException($"Bonus Type '{bonusType.Type}' already exists.");
            }

            var bonusTypeWithSameDisplayName =
                await _bonusTypeRepository.GetBonusTypeByDisplayNameAsync(bonusType.DisplayName);

            if (bonusTypeWithSameDisplayName != null)
            {
                throw new EntityAlreadyExistsException(
                    $"Bonus Type with same Display Name '{bonusType.DisplayName}' already exists.");
            }

            var bonusTypeResponse = await _bonusTypeRepository.InsertAsync(bonusType);

            _log.Info($"Bonus Type was added: {bonusType.ToJson()}", process: nameof(InsertAsync),
                context: bonusType.Type);

            return bonusTypeResponse;
        }

        public async Task UpdateAsync(BonusType bonusType)
        {
            var oldBonusType = await _bonusTypeRepository.GetBonusTypeAsync(bonusType.Type);

            if (oldBonusType == null)
            {
                throw new EntityNotFoundException($"Bonus Type with name {bonusType.Type} does not exist.");
            }

            var bonusTypeWithSameDisplayName =
                await _bonusTypeRepository.GetBonusTypeByDisplayNameAsync(bonusType.DisplayName);

            if (bonusTypeWithSameDisplayName != null && oldBonusType.Type != bonusTypeWithSameDisplayName.Type)
            {
                throw new EntityAlreadyExistsException(
                    $"Bonus Type with same Display Name '{bonusType.DisplayName}' already exists.");
            }

            // Copy old values to preserve them after update
            bonusType.CreationDate = oldBonusType.CreationDate;

            await _bonusTypeRepository.UpdateAsync(bonusType);

            _log.Info($"Bonus Type was updated: {bonusType.ToJson()}", process: nameof(UpdateAsync),
                context: bonusType.Type);
        }

        public async Task<BonusType> InsertOrUpdateAsync(BonusType bonusType)
        {
            var bonusTypeName = bonusType.Type.ToLower();

            var existingBonusType = await _bonusTypeRepository.GetBonusTypeAsync(bonusTypeName);

            if (existingBonusType == null)
            {
                return await InsertAsync(bonusType);
            }

            if (!existingBonusType.AreEqual(bonusType))
                await UpdateAsync(bonusType);

            return bonusType;
        }
    }
}
