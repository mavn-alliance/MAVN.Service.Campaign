using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Campaign.Domain.Models;

namespace Lykke.Service.Campaign.Domain.Services
{
    public interface IBonusTypeService
    {
        Task<IReadOnlyCollection<BonusType>> GetBonusTypesAsync();

        Task<BonusType> GetAsync(string bonusTypeName);

        Task<IReadOnlyCollection<BonusType>> GetActiveBonusTypesAsync();

        Task<BonusType> InsertAsync(BonusType bonusType);

        Task UpdateAsync(BonusType bonusType);

        Task<BonusType> InsertOrUpdateAsync(BonusType bonusType);
    }
}
