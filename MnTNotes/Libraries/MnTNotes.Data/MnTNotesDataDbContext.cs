using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MnTNotes.Core.Data;
using MnTNotes.Core.Data.Domain;
using System.IO;

namespace MnTNotes.Data
{
    public class MnTNotesDataDbContext : BaseIdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile(@Directory.GetCurrentDirectory() + "/appsettings.json")
              .Build();

            var connectionString = configuration.GetConnectionString("ConnectionSqlite");
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = connectionString
            };

            optionsBuilder.UseSqlite(connectionStringBuilder.ConnectionString);
        }

        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Note>(entity => { entity.ToTable(name: "Notes"); });
        }
    }
}