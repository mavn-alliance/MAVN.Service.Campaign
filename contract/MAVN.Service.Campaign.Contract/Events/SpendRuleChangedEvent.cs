using System;
using MAVN.Numerics;
using JetBrains.Annotations;
using MAVN.Service.Campaign.Contract.Enums;

namespace MAVN.Service.Campaign.Contract.Events
{
    /// <summary>
    /// Represent an event that is triggered once a spend rule is changed.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class SpendRuleChangedEvent
    {
        /// <summary>
        /// The event unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The spend rule unique identifier.
        /// </summary>
        public Guid SpendRuleId { get; set; }

        /// <summary>
        /// The spend rule title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The spend rule additional details.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The amount in tokens to calculate rate.
        /// </summary>
        public Money18? AmountInTokens { get; set; }

        /// <summary>
        /// The amount in currency to calculate rate.
        /// </summary>
        public decimal? AmountInCurrency { get; set; }

        /// <summary>
        /// Indicates that the global currency rate should be used to convert an amount.
        /// </summary>
        public bool UseGlobalCurrencyRate { get; set; }
        
        /// <summary>
        /// The timestamp of changes.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Represents the action that has triggered the event.
        /// </summary>
        public ActionType Action { get; set; }
    }
}
