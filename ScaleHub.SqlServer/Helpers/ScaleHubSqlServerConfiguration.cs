using ScaleHub.Core;
using System.Diagnostics.CodeAnalysis;

namespace ScaleHub.SqlServer.Helpers
{
    public class ScaleHubSqlServerConfiguration : ScaleHubConfiguration
    {
        public ScaleHubSqlServerConfiguration(ScaleHubConfiguration cfg)
        {
            this.ScaleHub = typeof(ScaleHubSqlServer);
            this.Subscription = cfg.Subscription;
            this.Tag = cfg.Tag;
        }

        public ScaleHubSqlServerConfiguration(ScaleHubConfiguration cfg, string connString)
        {
            this.ScaleHub = typeof(ScaleHubSqlServer);
            this.Subscription = cfg.Subscription;
            this.Tag = cfg.Tag;
            this.SqlServer.ConnString = connString;
        }

        public SqlServerConfiguration SqlServer { get; set; } = new();

        public class SqlServerConfiguration
        {
            /// <summary>
            /// Gets or sets the connection string for the database.
            /// </summary>
            public string ConnString { get; set; } = default!;

            /// <summary>
            /// periodically check settings.
            /// </summary>
            internal SqlPeriodicallyCheck PeriodicallyCheck { get; set; } = new SqlPeriodicallyCheck();
        }
    }

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
        /// </remarks>
        public TimeSpan MaxDiffAllowed { get; set; }


        internal LastUpdateComparer LastUpdateComparer => this.lastUpdateComparer ??= new LastUpdateComparer(MaxDiffAllowed);

        internal PeriodicTimer Timer => this.periodicTimer ??= new PeriodicTimer(Period);
    }

    internal readonly struct LastUpdateComparer : IEqualityComparer<DateTime>
    {
        private readonly long maxDiffAllowedTicks;

        public LastUpdateComparer(TimeSpan maxDiffAllowed)
        {
            this.maxDiffAllowedTicks = maxDiffAllowed.Ticks;
        }

        public bool Equals(DateTime x, DateTime y)
        {
            return this.maxDiffAllowedTicks >= Math.Abs((x - y).Ticks);
        }

        public int GetHashCode([DisallowNull] DateTime obj)
        {
            return 0;
        }
    }
}