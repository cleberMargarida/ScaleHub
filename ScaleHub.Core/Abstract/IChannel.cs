namespace ScaleHub.Core
{
    /// <summary>
    /// Represents a communication channel for managing events when replicas is added or removed.
    /// </summary>
    public interface IChannel
    {
        /// <summary>
        /// Occurs when a replica is added (subscribing).
        /// </summary>
        event ScaleHubEventHandler OnSubscribing;

        /// <summary>
        /// Occurs when a replica is removed (unsubscribing).
        /// </summary>
        event ScaleHubEventHandler OnUnsubscribing;
    }
}