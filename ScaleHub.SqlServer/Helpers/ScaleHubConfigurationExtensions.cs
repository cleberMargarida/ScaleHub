using ScaleHub.Core;
using ScaleHub.SqlServer.Configurations;

namespace ScaleHub.SqlServer.Helpers
{
    /// <summary>
    /// Provides extension methods for configuring ScaleHub with SQL Server.
    /// </summary>
    public static class ScaleHubConfigurationExtensions
    {
        /// <summary>
        /// Configures the ScaleHub to use SQL Server with the specified connection string.
        /// </summary>
        /// <param name="cfg">The <see cref="ScaleHubConfiguration"/> to configure.</param>
        /// <param name="connString">The connection string for the SQL Server database.</param>
        public static void UseSqlServer(this ScaleHubConfiguration cfg, string connString)
        {
            ScaleHubConfiguration.Singleton = new ScaleHubSqlServerConfiguration(cfg, connString);
        }

        /// <summary>
        /// Configures the ScaleHub to use SQL Server with the specified action.
        /// </summary>
        /// <param name="cfg">The <see cref="ScaleHubConfiguration"/> to configure.</param>
        /// <param name="action">The action to apply for the SQL Server database connection.</param>
        public static void UseSqlServer(this ScaleHubConfiguration cfg, Action<ScaleHubSqlServerConfiguration.SqlServerConfiguration> action)
        {
            var scaleHubSqlServerConfiguration = new ScaleHubSqlServerConfiguration(cfg);
            action(scaleHubSqlServerConfiguration.SqlServer);
            ScaleHubConfiguration.Singleton = scaleHubSqlServerConfiguration;
        }

        /// <summary>
        /// Configures the periodically check settings for the ScaleHub SQL Server configuration.
        /// </summary>
        /// <param name="cfg">The SQL Server configuration to be updated.</param>
        /// <param name="action">An action to specify the periodically check settings.</param>
        /// <remarks>
        /// The periodically check is used for recurring check for changes on the servers. 
        /// Can be useful when having services stopping abruptly, before notify a down.
        /// </remarks>
        public static void PeriodicallyCheck(this ScaleHubSqlServerConfiguration.SqlServerConfiguration cfg, Action<SqlPeriodicallyCheck> action)
        {
            action(cfg.PeriodicallyCheck);
            cfg.PeriodicallyCheck.Enabled = true;
            cfg.Validate();
        }

    }
}
