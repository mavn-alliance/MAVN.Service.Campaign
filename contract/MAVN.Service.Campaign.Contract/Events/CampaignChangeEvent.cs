using System;
using MAVN.Service.Campaign.Contract.Enums;

namespace MAVN.Service.Campaign.Contract.Events
{
    // TODO: Rename to earn rule change event.
    /// <summary>
    /// Represent an event that is triggered once an earn rule changed.
    /// </summary>
    public class CampaignChangeEvent
    {
        /// <summary>
        /// Represents event's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents the id of the changed campaign
        /// </summary>
        public Guid CampaignId { get; set; }

        /// <summary>
        /// Represents change's time stamp
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Represents campaign's current status
        /// </summary>
        public CampaignStatus Status { get; set; }

        /// <summary>
        /// Represents tha Action that has triggered the event
        /// </summary>
        public ActionType Action { get; set; }
    }
}
