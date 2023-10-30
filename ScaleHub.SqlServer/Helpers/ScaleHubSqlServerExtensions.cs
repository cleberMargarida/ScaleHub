using ScaleHub.Core;

namespace ScaleHub.SqlServer.Helpers
{
    /// <summary>
    /// Provides extension methods for working with ScaleHub SQL Server specific functionality.
    /// </summary>
    internal static class ScaleHubSqlServerExtensions
    {
        /// <summary>
        /// Finds a server in a collection of servers that matches the given server's IP and hostname.
        /// </summary>
        /// <param name="servers">The collection of servers to search in.</param>
        /// <param name="server">The server to find within the collection.</param>
        /// <returns>The <see cref="ServerInfo"/> representing the found server, or null if not found.</returns>
        public static ServerInfo FindServer(this IEnumerable<ServerInfo> servers, ServerInfo server)
        {
            return servers.First(s => s.Ip == server.Ip && s.HostName == server.HostName);
        }
    }
}
