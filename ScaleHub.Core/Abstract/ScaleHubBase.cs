using ScaleHub.Core;

namespace ScaleHub.SqlServer
{
    /// <summary>
    /// An abstract base class for managing subscriptions.
    /// </summary>
    internal abstract class ScaleHubBase : IScaleHub
    {
        /// <inheritdoc/>
        public event ScaleHubEventHandler? OnSubscribing;

        /// <inheritdoc/>
        public event ScaleHubEventHandler? OnUnsubscribing;

        /// <inheritdoc/>
        public abstract ScaleContext GetContext();

        /// <inheritdoc/>
        public abstract Task Subscribe(CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract Task Unsubscribe(CancellationToken cancellationToken);


        /// <summary>
        /// Raises the OnSubscribing event asynchronously.
        /// </summary>
        protected async Task RaiseOnSubscribe()
        {
            if (OnSubscribing == null)
            {
                return;
            }

            var context = GetContext();
            await OnSubscribing(context);
        }

        /// <summary>
        /// Raises the OnUnsubscribing event asynchronously.
        /// </summary>
        protected async Task RaiseOnUnsubscribe()
        {
            if (OnUnsubscribing == null)
            {
                return;
            }

            var context = GetContext();
            await OnUnsubscribing(context);
        }

        /// <summary>
        /// Start to listen for replicas subscribing and unsubscribing.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the operation.</param>
        protected abstract Task ListenForChanges(CancellationToken cancellationToken);

        /// <summary>
        /// Gets information about the replicas.
        /// </summary>
        /// <returns>The <see cref="ServerInfo"/> representing information about the replicas.</returns>
        protected abstract ServerInfo GetServerInfo();
    }
}