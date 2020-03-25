namespace Lykke.Service.Campaign.Contract.Enums
{
    /// <summary>
    /// Specifies an entity change action. 
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Indicates that the entity was created.
        /// </summary>
        Created,

        /// <summary>
        /// Indicates that the entity was updated.
        /// </summary>
        Edited,

        /// <summary>
        /// Indicates that the entity status changed to active.
        /// </summary>
        Activated,

        /// <summary>
        /// Indicates that the entity status changed to completed.
        /// </summary>
        Completed,

        /// <summary>
        /// Indicates that the entity was deleted.
        /// </summary>
        Deleted
    }
}
