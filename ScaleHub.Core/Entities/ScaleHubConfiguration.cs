namespace ScaleHub.Core
{
    /// <summary>
    /// Represents the configuration for a scaling hub.
    /// </summary>
    public class ScaleHubConfiguration
    {
        /// <summary>
        /// Gets the default configuration for a scaling hub.
        /// </summary>
        internal static ScaleHubConfiguration Default { get; } = new ScaleHubConfiguration();

        /// <summary>
        /// Gets or sets the type of the scaling hub.
        /// </summary>
        internal Type ScaleHub { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subscription action for the scaling hub.
        /// </summary>
        internal Action<IChannel> Subscription { get; set; } = default!;

        /// <summary>
        /// Configures the subscription action for the scaling hub.
        /// </summary>
        /// <remarks>
        /// [optional]
        /// </remarks>
        /// <param name="subscription">The action to configure the subscription for the scaling hub.</param>
        public void ConfigureSubs(Action<IChannel> subscription)
        {
            Subscription = subscription;
        }
    }

}