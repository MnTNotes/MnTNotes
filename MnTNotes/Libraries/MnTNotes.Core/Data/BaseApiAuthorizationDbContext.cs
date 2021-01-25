using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MnTNotes.Core.Data.Domain;
using MnTNotes.Core.Data.Domain.Identity;

namespace MnTNotes.Core.Data
{
    public abstract class BaseApiAuthorizationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IDbContext
    {
        public BaseApiAuthorizationDbContext(
           DbContextOptions options,
           IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<IdentityRole> IdentityRoles { get; set; }
        public DbSet<IdentityUserRole<string>> IdentityUserRoles { get; set; }
        public DbSet<IdentityUserClaim<string>> IdentityUserClaims { get; set; }
        public DbSet<IdentityUserLogin<string>> IdentityUserLogins { get; set; }
        public DbSet<IdentityUserToken<string>> IdentityUserTokens { get; set; }
        public DbSet<IdentityRoleClaim<string>> IdentityRoleClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Note>(entity => { entity.ToTable(name: "Notes"); });

            // Override default AspNet Identity table names
            builder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserTokens"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaims"); });
        }
    }
}