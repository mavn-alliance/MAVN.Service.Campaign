using System.Collections.Generic;
using System.Threading.Tasks;
using MAVN.Service.Campaign.Domain.Models;

namespace MAVN.Service.Campaign.Domain.Repositories
{
    public interface IBonusTypeRepository
    {
        Task<BonusType> GetBonusTypeAsync(string bonusTypeName);

        Task<BonusType> GetBonusTypeByDisplayNameAsync(string bonusTypeDisplayName);

        Task<BonusType> InsertAsync(BonusType bonusType);

        Task UpdateAsync(BonusType bonusType);

        Task<IReadOnlyCollection<BonusType>> GetBonusTypesAsync();

        Task<IReadOnlyCollection<BonusType>> GetActiveBonusTypesAsync();
    }
}
