﻿using Microsoft.EntityFrameworkCore;
using ScaleHub.Core;

namespace ScaleHub.SqlServer.Data
{
    /// <summary>
    /// Represents the Entity Framework context for ScaleHub data.
    /// </summary>
    internal class ScaleHubDbContext : DbContext
    {
        /// <summary>
        /// Gets or sets the connection string for the database.
        /// </summary>
        internal static string ConnString { get; set; } = default!;

        /// <summary>
        /// Gets or sets the DbSet for storing server information.
        /// </summary>
        public DbSet<ServerInfo> Servers { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}