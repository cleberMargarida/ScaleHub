using ScaleHub.Core;
using ScaleHub.SqlServer.Data;

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
        /// <returns>The updated <see cref="ScaleHubConfiguration"/>.</returns>
        public static ScaleHubConfiguration UseSqlServer(this ScaleHubConfiguration cfg, string connString)
        {
            ScaleHubDbContext.ConnString = connString;
            cfg.ScaleHub = typeof(ScaleHubSqlServer);
            return cfg;
        }
    }
}
