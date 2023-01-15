using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MnTNotes.Core.Data.Domain;
using MnTNotes.Core.Data.Domain.Identity;

namespace MnTNotes.Core.Data
{
    public abstract class BaseIdentityDbContext :
        IdentityDbContext<ApplicationUser, ApplicationRole, string,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> IdentityRoles { get; set; }
        public DbSet<ApplicationUserClaim> IdentityUserClaims { get; set; }
        public DbSet<ApplicationUserRole> IdentityUserRoles { get; set; }
        public DbSet<ApplicationUserLogin> IdentityUserLogins { get; set; }
        public DbSet<ApplicationRoleClaim> IdentityRoleClaims { get; set; }
        public DbSet<ApplicationUserToken> IdentityUserTokens { get; set; }

        // ApiAuthorization Tables
        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Note>(entity => { entity.ToTable(name: "Notes"); });

            // Override default AspNet Identity table names
            modelBuilder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
            modelBuilder.Entity<ApplicationRole>(entity => { entity.ToTable(name: "Roles"); });
            modelBuilder.Entity<ApplicationUserClaim>(entity => { entity.ToTable("UserClaims"); });
            modelBuilder.Entity<ApplicationUserRole>(entity => { entity.ToTable("UserRoles"); });
            modelBuilder.Entity<ApplicationUserLogin>(entity => { entity.ToTable("UserLogins"); });
            modelBuilder.Entity<ApplicationRoleClaim>(entity => { entity.ToTable("RoleClaims"); });
            modelBuilder.Entity<ApplicationUserToken>(entity => { entity.ToTable("UserTokens"); });

            modelBuilder.Entity<PersistedGrant>().HasKey(entity => entity.Key);
            modelBuilder.Entity<DeviceFlowCodes>().HasKey(entity => entity.DeviceCode);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                // Each User can have many UserClaims
                //entity.HasMany(e => e.Claims)
                //    .WithOne(e => e.User)
                //    .HasForeignKey(uc => uc.UserId)
                //    .IsRequired();

                // Each User can have many UserLogins
                entity.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                entity.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                entity.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(entity =>
            {
                // Each Role can have many entries in the UserRole join table
                entity.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                entity.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });
        }
    }
}