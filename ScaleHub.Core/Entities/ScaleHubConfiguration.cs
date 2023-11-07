using ScaleHub.Core.Abstract;

namespace ScaleHub.Core
{
    /// <summary>
    /// Represents the configuration for a scaling hub.
    /// </summary>
    public class ScaleHubConfiguration : ISetup
    {
        /// <summary>
        /// Gets the default configuration for a scaling hub.
        /// </summary>
        internal static ScaleHubConfiguration Singleton { get; set; } = new ScaleHubConfiguration();

        protected ScaleHubConfiguration() { }

        /// <summary>
        /// Gets or sets the type of the scaling hub.
        /// </summary>
        internal Type ScaleHub { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Tag to differ the service.
        /// </summary>
        /// <remarks>
        /// [Optional]
        /// The current domain friendly name is used as the default value.
        /// </remarks>
        public string Tag { get; set; } = AppDomain.CurrentDomain.FriendlyName;

        /// <summary>
        /// Gets or sets the subscription action for the scaling hub.
        /// </summary>
        internal Action<IChannel> Subscription { get; set; } = default!;

        /// <summary>
        /// Configures the subscription action for the scaling hub.
        /// </summary>
        /// <remarks>
        /// [Optional]
        /// </remarks>
        /// <param name="subscription">The action to configure the subscription for the scaling hub.</param>
        public void ConfigureSubs(Action<IChannel> subscription)
        {
            Singleton.Subscription = subscription;
        }
    }

}