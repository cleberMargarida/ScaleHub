using Microsoft.EntityFrameworkCore;
using ScaleHub.Core;

namespace ScaleHub.SqlServer.Data
{
    /// <summary>
    /// Represents the Entity Framework context for ScaleHub data.
    /// </summary>
    internal class ScaleHubDbContext : DbContext
    {
        internal const string ServersTable = "scale_servers";
        private readonly string connString;

        public ScaleHubDbContext(string connString)
        {
            this.connString = connString;
        }

        /// <summary>
        /// Gets or sets the DbSet for storing server information.
        /// </summary>
        public DbSet<ServerInfo> Servers { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServerInfo>().ToTable(ServersTable);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
