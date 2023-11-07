#pragma warning disable CS4014

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScaleHub.Core;
using ScaleHub.Core.Abstract;
using ScaleHub.SqlServer.Configurations;
using ScaleHub.SqlServer.Data;
using ScaleHub.SqlServer.Helpers;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace ScaleHub.SqlServer
{
    /// <summary>
    /// Implementation of ScaleHub for SQL Server.
    /// </summary>
    internal class ScaleHubSqlServer : ScaleHubBase, IDisposable
    {
        private readonly ScaleHubSqlServerConfiguration setup;
        private readonly ILogger<ScaleHubSqlServer> logger;
        private readonly ScaleHubDbContext context;
        private readonly Task initializing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleHubSqlServer"/> class.
        /// </summary>
        public ScaleHubSqlServer(ISetup setup, ILogger<ScaleHubSqlServer> logger)
        {
            this.setup = (ScaleHubSqlServerConfiguration)setup;
            this.logger = logger;
            this.setup.Subscription(this);
            this.context = new ScaleHubDbContext(this.setup.SqlServer.ConnString);
            this.initializing = EnsureDatabaseDependenciesAsync();
        }

        /// <inheritdoc />
        public override ScaleContext GetContext()
        {
            var servers = this.context.Servers.Where(s => s.Tag == this.setup.Tag)
                                              .ToList();

            return GetContext(servers);
        }

        /// <inheritdoc />
        public override async Task<ScaleContext> GetContextAsync()
        {
            var servers = await this.context.Servers.Where(s => s.Tag == this.setup.Tag)
                                                    .ToListAsync();

            return GetContext(servers);
        }

        private static ScaleContext GetContext(List<ServerInfo> servers)
        {
            var actual = servers.First(s => s.Ip == Ip && s.HostName == HostName);
            var actualIndex = servers.IndexOf(actual);

            return new ScaleContext(servers.Count, ++actualIndex);
        }

        /// <inheritdoc />
        public override async Task Subscribe(CancellationToken cancellationToken)
        {
            await initializing;

            var entity = CreateEntity();
            await this.context.Servers.AddAsync(entity, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);

            if (base.HasEvents)
            {
                ListenForChanges(cancellationToken);
            }

            if (this.setup.SqlServer.PeriodicallyCheck.Enabled)
            {
                PeriodicallyCheck(cancellationToken);
            }
        }

        /// <inheritdoc />
        public override async Task Unsubscribe(CancellationToken cancellationToken)
        {
            var server = this.context.Servers.Where(s => s.Tag == this.setup.Tag)
                                             .First(s => s.Ip == Ip && s.HostName == HostName);

            context.Servers.Remove(server);
            await this.context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        protected override async Task ListenForChanges(CancellationToken cancellationToken)
        {
            using var tableDependecy = new SqlTableDependency<ServerInfo>(
                  this.setup.SqlServer.ConnString
                , ScaleHubDbContext.ServersTable);

            tableDependecy.OnChanged += NotifyChange;
            tableDependecy.Start();

            await Task.Delay(Timeout.Infinite, cancellationToken);
            tableDependecy.Stop();
        }

        /// <summary>
        /// Ensures that the necessary database dependencies are in place, 
        /// including database schema creation and enabling SQL Service Broker.
        /// </summary>
        /// <remarks>
        /// This method continuously attempts to ensure database dependencies until success or a timeout. 
        /// If an exception occurs during the process, it logs an error message and retries after a delay.
        /// </remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task EnsureDatabaseDependenciesAsync()
        {
            while (true)
            {
                try
                {
                    await this.context.Database.EnsureCreatedAsync();
                    await this.context.Database.EnsureBrokerEnabled();
                    break;
                }
                catch (SqlException)
                {
                    logger.LogError("EnsureCreatedAsync failed. Retrying again in 500ms.");
                    await Task.Delay(500);
                }
            }
        }

        /// <summary>
        /// Notifies a change in the database record.
        /// </summary>
        /// <param name="sender">The sender of the notification.</param>
        /// <param name="e">The event arguments containing information about the record change.</param>
        private void NotifyChange(object sender, RecordChangedEventArgs<ServerInfo> e)
        {
            if (e.Entity.Tag != this.setup.Tag)
            {
                return;
            }

            NotifyChange(e);
        }

        /// <summary>
        /// Notifies a change in the database record asynchronously.
        /// </summary>
        /// <param name="e">The event arguments containing information about the record change.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        private async Task NotifyChange(RecordChangedEventArgs<ServerInfo> e) => await (e.ChangeType switch
        {
            ChangeType.Insert => RaiseOnSubscribe(),
            ChangeType.Delete => RaiseOnUnsubscribe(),
            _ => Task.CompletedTask
        });

        /// <summary>
        /// Periodically checks the SQL Server and updates actual data while removing inactive items.
        /// This method runs in a loop until cancellation is requested through the provided CancellationToken.
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken that can be used to request cancellation of the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task PeriodicallyCheck(CancellationToken cancellationToken)
        {
            while (await this.setup.SqlServer.PeriodicallyCheck.Timer.WaitForNextTickAsync(cancellationToken) &&
                  !cancellationToken.IsCancellationRequested)
            {
                await UpdateActualAndRemoveInactives(cancellationToken);
            }
        }

        /// <summary>
        /// Updates actual server data and removes inactive servers from the database.
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken that can be used to request cancellation of the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task UpdateActualAndRemoveInactives(CancellationToken cancellationToken)
        {
            var servers = await GetServers(cancellationToken);

            var activesServers = servers.OrderByDescending(s => s.LastUpdate)
                                        .GroupBy(s => s.LastUpdate, this.setup.SqlServer.PeriodicallyCheck.LastUpdateComparer)
                                        .First()
                                        .AsEnumerable();

            var inactiveServers = servers.Except(activesServers);

            var entity = servers.First(s => s.Ip == Ip && s.HostName == HostName);

            entity.LastUpdate = DateTime.UtcNow;

            this.context.Servers.Update(entity);
            this.context.Servers.RemoveRange(inactiveServers);
            await this.context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a list of server information from the database that match a specified tag.
        /// </summary>
        /// <param name="cancellationToken">A CancellationToken that can be used to request cancellation of the operation.</param>
        /// <returns>A List of ServerInfo representing server information retrieved from the database.</returns>
        private async Task<List<ServerInfo>> GetServers(CancellationToken cancellationToken)
        {
            var servers = await this.context.Servers.Where(s => s.Tag == this.setup.Tag)
                                                    .ToListAsync(cancellationToken);

            await EntityEntryReload(servers, cancellationToken);

            return servers;
        }

        /// <summary>
        /// Reloads the Entity Entry states of a list of ServerInfo entities asynchronously.
        /// </summary>
        /// <param name="servers">A list of ServerInfo entities whose Entity Entry states need to be reloaded.</param>
        /// <param name="cancellationToken">A CancellationToken that can be used to request cancellation of the operation.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private async Task EntityEntryReload(List<ServerInfo> servers, CancellationToken cancellationToken)
        {
            var tasks = GetEntityEntryReloadTasks(servers, cancellationToken);
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Generates a collection of asynchronous tasks to reload the Entity Entry states for a list of ServerInfo entities.
        /// </summary>
        /// <param name="servers">A list of ServerInfo entities whose Entity Entry states need to be reloaded.</param>
        /// <param name="cancellationToken">A CancellationToken that can be used to request cancellation of the operation.</param>
        /// <returns>An IEnumerable of Task representing the asynchronous reload operations.</returns>
        private IEnumerable<Task> GetEntityEntryReloadTasks(List<ServerInfo> servers, CancellationToken cancellationToken)
        {
            foreach (var server in servers)
            {
                yield return context.Entry(server).ReloadAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Creates and returns a new ServerInfo entity with specified properties.
        /// </summary>
        /// <returns>A new ServerInfo entity with the specified properties set.</returns>
        private ServerInfo CreateEntity() => new()
        {
            Ip = Ip,
            HostName = HostName,
            LastUpdate = DateTime.UtcNow,
            Tag = this.setup.Tag,
        };


        /// <inheritdoc />
        public void Dispose()
        {
            this.context.Dispose();
            this.setup.SqlServer.PeriodicallyCheck.Timer.Dispose();
        }
    }
}
