using JetBrains.Annotations;
using MAVN.Service.Campaign.Client.Models.BonusType;
using Refit;
using System.Threading.Tasks;

namespace MAVN.Service.Campaign.Client.Api
{
    /// <summary>
    /// BonusType API
    /// </summary>
    [PublicAPI]
    public interface IBonusTypesApi
    {
        /// <summary>
        /// Returns List of available Bonus Types
        /// </summary>
        [Get("/api/bonusTypes")]
        Task<BonusTypeListResponseModel> GetAsync();

        /// <summary>
        /// Returns a bonusType model by type
        /// </summary>
        [Get("/api/bonusTypes/{type}")]
        Task<BonusTypeModel> GetByTypeAsync(string type);

        /// <summary>
        /// Returns list of all active Bonus Types.
        /// </summary>
        [Get("/api/bonusTypes/active")]
        Task<BonusTypeListResponseModel> GetActiveAsync();
    }
}
