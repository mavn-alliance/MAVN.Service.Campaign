using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Campaign.Client.Models.Condition;
using Refit;

namespace Lykke.Service.Campaign.Client.Api
{
    /// <summary>
    /// Provides methods to work with conditions.
    /// </summary>
    [PublicAPI]
    public interface IConditionsApi
    {
        /// <summary>
        /// Returns a Condition by conditionId.
        /// </summary>
        /// <returns>ConditionDetailResponseModel</returns>
        [Get("/api/conditions/{conditionId}")]
        Task<ConditionDetailsResponseModel> GetByIdAsync(string conditionId);
    }
}
