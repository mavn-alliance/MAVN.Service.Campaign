using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lykke.Service.Campaign.Client.Models.BonusType
{
    /// <summary>
    /// Represents entity containing list of Condition Types 
    /// </summary>
    [PublicAPI]
    public class BonusTypeListResponseModel
    {
        /// <summary>
        /// List of Bonus Types
        /// </summary>
        public IReadOnlyCollection<BonusTypeModel> BonusTypes { get; set; }
    }
}
