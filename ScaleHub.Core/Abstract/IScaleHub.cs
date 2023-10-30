namespace ScaleHub.Core
{
    /// <summary>
    /// Represents a delegate that handles subscriptions and unsubscriptions.
    /// </summary>
    /// <param name="context">The scale context for the event.</param>
    public delegate Task ScaleHubEventHandler(ScaleContext context);

    /// <summary>
    /// Represents a scale hub interface for managing subscriptions and unsubscriptions.
    /// </summary>
    public interface IScaleHub : IChannel
    {
        /// <summary>
        /// Asynchronously subscribes to the scale hub.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the subscription.</param>
        internal Task Subscribe(CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously unsubscribes from the scale hub.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the unsubscription.</param>
        internal Task Unsubscribe(CancellationToken cancellationToken);

        /// <summary>
        /// Gets the current scale context.
        /// </summary>
        /// <returns>The <see cref="ScaleContext"/> representing the current context.</returns>
        ScaleContext GetContext();
    }

}