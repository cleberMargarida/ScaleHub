#pragma warning disable CS4014

using Microsoft.Data.SqlClient;
using ScaleHub.Core;
using ScaleHub.SqlServer.Data;
using ScaleHub.SqlServer.Helpers;
using System.Net;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace ScaleHub.SqlServer
{
    /// <summary>
    /// Implementation of ScaleHub for SQL Server.
    /// </summary>
    internal class ScaleHubSqlServer : ScaleHubBase, IScaleHub, IDisposable
    {
        private readonly ScaleHubDbContext context;
        private readonly ServerInfo server;
        private readonly Task initializing;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleHubSqlServer"/> class.
        /// </summary>
        public ScaleHubSqlServer()
        {
            this.context = new ScaleHubDbContext();
            this.server = GetServerInfo();
            this.initializing = EnsureDatabaseDependenciesAsync();
        }

        /// <inheritdoc />
        public override ScaleContext GetContext()
        {
            var servers = this.context.Servers.ToList();
            var actual = servers.FindServer(server);
            var actualIndex = servers.IndexOf(actual);
            return new ScaleContext { Actual = actualIndex + 1, Replicas = servers.Count };
        }

        /// <inheritdoc />
        public override async Task Subscribe(CancellationToken cancellationToken)
        {
            await initializing;
            await this.context.Servers.AddAsync(server, cancellationToken);
            await this.context.SaveChangesAsync(cancellationToken);
            ListenForChanges(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task Unsubscribe(CancellationToken cancellationToken)
        {
            var server = context.Servers.FindServer(this.server);
            context.Servers.Remove(server);
            await this.context.SaveChangesAsync(cancellationToken);
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
                    Console.WriteLine("EnsureCreatedAsync failed. Retrying again in 500ms.");
                    await Task.Delay(500);
                }
            }
        }

        /// <inheritdoc />
        protected override async Task ListenForChanges(CancellationToken cancellationToken)
        {
            using var tableDependecy = new SqlTableDependency<ServerInfo>(
                  ScaleHubDbContext.ConnString
                , nameof(ScaleHubDbContext.Servers));

            tableDependecy.OnChanged += NotifyChange;
            tableDependecy.Start();

            await Task.Delay(Timeout.Infinite, cancellationToken);
            tableDependecy.Stop();
        }

        /// <summary>
        /// Notifies a change in the database record.
        /// </summary>
        /// <param name="sender">The sender of the notification.</param>
        /// <param name="e">The event arguments containing information about the record change.</param>
        private void NotifyChange(object sender, RecordChangedEventArgs<ServerInfo> e)
        {
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


        /// <inheritdoc />
        protected override ServerInfo GetServerInfo()
        {
            var hostName = Dns.GetHostName();
            var hostEntry = Dns.GetHostEntry(hostName);
            return new ServerInfo { HostName = hostName, Ip = hostEntry.AddressList[0].ToString() };
        }

        /// <inheritdoc />
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
