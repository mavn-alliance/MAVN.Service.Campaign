using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Lykke.Service.Campaign.Client.Models.BurnRule.Requests
{
    /// <inheritdoc />
    [PublicAPI]
    public class BurnRulePaginationRequest : BasePaginationRequestModel
    {
        /// <summary>
        /// Represents search field by burn rule's title (optional)
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Represents search field by burn rule's partner's (optional)
        /// </summary>
        public Guid? PartnerId { get; set; }
    }
}
