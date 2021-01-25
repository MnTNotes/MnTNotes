using Microsoft.EntityFrameworkCore;
using MnTNotes.Core.Data.Domain;

namespace MnTNotes.Core.Data
{
    public abstract class BaseDbContext : DbContext, IDbContext
    {
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Note>(entity => { entity.ToTable(name: "Notes"); });
        }

    }
}