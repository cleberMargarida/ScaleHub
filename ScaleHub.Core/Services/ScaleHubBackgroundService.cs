using Microsoft.Extensions.Hosting;

namespace ScaleHub.Core
{
    /// <summary>
    /// Represents a background service for managing a scale hub.
    /// </summary>
    internal class ScaleHubBackgroundService : IHostedService
    {
        private readonly IScaleHub hub;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleHubBackgroundService"/> class.
        /// </summary>
        /// <param name="hub">The scale hub to manage.</param>
        public ScaleHubBackgroundService(IScaleHub hub)
        {
            this.hub = hub;
        }

        /// <summary>
        /// Starts the background service and subscribes to the scale hub.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to stop the service.</param>
        public async Task StartAsync(CancellationToken cancellationToken) => await hub.Subscribe(cancellationToken);

        /// <summary>
        /// Stops the background service and unsubscribes from the scale hub.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to stop the service.</param>
        public async Task StopAsync(CancellationToken cancellationToken) => await hub.Unsubscribe(cancellationToken);
    }

}
