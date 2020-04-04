using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Campaign.Domain.Models;

namespace MAVN.Service.Campaign.Domain.Services
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
