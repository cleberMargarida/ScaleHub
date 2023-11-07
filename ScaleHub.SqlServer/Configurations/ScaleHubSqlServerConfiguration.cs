using ScaleHub.Core;
using ScaleHub.SqlServer.Exceptions;

namespace ScaleHub.SqlServer.Configurations
{
    public class ScaleHubSqlServerConfiguration : ScaleHubConfiguration
    {
        public ScaleHubSqlServerConfiguration(ScaleHubConfiguration cfg)
        {
            ScaleHub = typeof(ScaleHubSqlServer);
            Subscription = cfg.Subscription;
            Tag = cfg.Tag;
        }

        public ScaleHubSqlServerConfiguration(ScaleHubConfiguration cfg, string connString)
        {
            ScaleHub = typeof(ScaleHubSqlServer);
            Subscription = cfg.Subscription;
            Tag = cfg.Tag;
            SqlServer.ConnString = connString;
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

            internal void Validate()
            {
                if (string.IsNullOrWhiteSpace(ConnString))
                {
                    throw new SqlServerConfigurationValidationException("The provided ConnString is empty.");
                }

                if ((TimeSpan.Zero == PeriodicallyCheck.MaxDiffAllowed || TimeSpan.Zero == PeriodicallyCheck.Period)
                     && PeriodicallyCheck.Enabled)
                {
                    throw new SqlServerConfigurationValidationException("The MaxDiffAllowed and Period needs be greater than Zero.");
                }

                if (PeriodicallyCheck.Enabled
                    && PeriodicallyCheck.MaxDiffAllowed <= PeriodicallyCheck.Period)
                {
                    throw new SqlServerConfigurationValidationException("The MaxDiffAllowed should be greater than the Period.");
                }
            }
        }
    }
}