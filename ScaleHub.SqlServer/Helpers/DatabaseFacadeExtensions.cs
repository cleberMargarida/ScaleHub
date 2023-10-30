using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection;

namespace ScaleHub.SqlServer.Helpers
{
    /// <summary>
    /// Provides extension methods for working with the Entity Framework database facade.
    /// </summary>
    internal static class DatabaseFacadeExtensions
    {
        /// <summary>
        /// Ensures that the SQL Service Broker is enabled for the database.
        /// </summary>
        /// <param name="databaseFacade">The <see cref="DatabaseFacade"/> instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal static async Task EnsureBrokerEnabled(this DatabaseFacade databaseFacade)
        {
            var database = databaseFacade.GetDbConnection()
                                         .Database;

            using var stream = Assembly.GetExecutingAssembly()
                                       .GetManifestResourceStream("ScaleHub.SqlServer.Data.EnableBroker.sql")!;

            using var reader = new StreamReader(stream);
            var sql = await reader.ReadToEndAsync();

            await databaseFacade.ExecuteSqlRawAsync(string.Format(sql, database));
        }
    }
}
