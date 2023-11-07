using ScaleHub.SqlServer.Helpers;

namespace ScaleHub.SqlServer.Configurations
{
    public class SqlPeriodicallyCheck
    {
        private LastUpdateComparer? lastUpdateComparer;
        private PeriodicTimer? periodicTimer;

        /// <summary>
        /// Gets or sets a value indicating whether the periodically check feature is enabled.
        /// </summary>
        /// <remarks>
        /// Activated when <see cref="ScaleHubConfigurationExtensions.PeriodicallyCheck"/> called.
        /// <para>
        /// <see cref="false"/> as default.
        /// </para>
        /// </remarks>
        internal bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the time interval between invocations of the periodically check mechanism.
        /// </summary>
        /// <remarks>
        /// [Mandatory].
        /// </remarks>
        public TimeSpan Period { get; set; }

        /// <summary>
        /// Gets or sets the maximum allowed time difference between different last update datetimes.
        /// </summary>
        /// <remarks>
        /// This property defines the maximum allowable time difference between the last update datetimes of servers when performing periodically checks.
        /// It ensures that the time difference between updates is within an acceptable range.
        /// It is used in the context of the periodically check feature to recognize servers that stopped abruply 
        /// without send a down signal, if a server has a difference on the last update datetime related to other services
        /// bigger than the configured here, the service will be considered stopped.
        /// <para>
        /// This needs to be greater than <see cref="Period"/>
        /// </para>
        /// <para>
        /// [Mandatory].
        /// </para>
        /// </remarks>
        public TimeSpan MaxDiffAllowed { get; set; }


        internal LastUpdateComparer LastUpdateComparer => lastUpdateComparer ??= new LastUpdateComparer(MaxDiffAllowed);

        internal PeriodicTimer Timer => periodicTimer ??= new PeriodicTimer(Period);
    }
}