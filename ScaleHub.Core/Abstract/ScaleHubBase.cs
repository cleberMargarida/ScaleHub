using ScaleHub.Core;
using ScaleHub.Core.Abstract;
using System.Net;

namespace ScaleHub.SqlServer
{
    /// <summary>
    /// An abstract base class for managing subscriptions.
    /// </summary>
    internal abstract class ScaleHubBase : IScaleHub, IChannel
    {
        /// <inheritdoc/>
        public event ScaleHubEventHandler? OnSubscribing;

        /// <inheritdoc/>
        public event ScaleHubEventHandler? OnUnsubscribing;
        
        /// <inheritdoc/>
        public abstract ScaleContext GetContext();

        /// <inheritdoc/>
        public abstract Task<ScaleContext> GetContextAsync();
        
        /// <inheritdoc/>
        public abstract Task Subscribe(CancellationToken cancellationToken);

        /// <inheritdoc/>
        public abstract Task Unsubscribe(CancellationToken cancellationToken);

        protected bool HasEvents => this.OnSubscribing != null || this.OnUnsubscribing != null;

        protected static string HostName => Dns.GetHostName();

        protected static string Ip => Dns.GetHostEntry(HostName).AddressList[0].ToString();


        /// <summary>
        /// Raises the OnSubscribing event asynchronously.
        /// </summary>
        protected async Task RaiseOnSubscribe()
        {
            if (OnSubscribing == null)
            {
                return;
            }

            var context = await GetContextAsync();
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

            var context = await GetContextAsync();
            await OnUnsubscribing(context);
        }

        /// <summary>
        /// Start to listen for replicas subscribing and unsubscribing.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the operation.</param>
        protected abstract Task ListenForChanges(CancellationToken cancellationToken);
    }
}