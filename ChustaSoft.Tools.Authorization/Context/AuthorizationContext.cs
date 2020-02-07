using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;


namespace ChustaSoft.Tools.Authorization
{
    public class AuthorizationContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        
        #region Constants

        private const string SCHEMA_NAME = "Auth";
        private const int MAX_FULL_VARCHAR_LENGTH = 256;
        private const int MAX_HALF_VARCHAR_LENGTH = 128;

        #endregion


        #region Constructor

        public AuthorizationContext(DbContextOptions<AuthorizationContext> options) : base(options) { }

        #endregion


        #region Protected methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
                entity.Property(e => e.Name).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.Property(e => e.NormalizedName).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasIndex(e => e.NormalizedName).IsUnique().HasName("RoleNameIndex");

                entity.ToTable($"{nameof(Role)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.RoleId).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.RoleId);

                entity.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(RoleClaim)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
                entity.Property(e => e.UserName).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.Property(e => e.EmailConfirmed);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.Property(e => e.NormalizedUserName).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.NormalizedEmail).IsUnique().HasName("EmailIndex");
                entity.HasIndex(e => e.NormalizedUserName).IsUnique().HasName("UserNameIndex");

                entity.ToTable($"{nameof(User)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);

                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(UserClaim)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.Property(e => e.LoginProvider).HasMaxLength(MAX_HALF_VARCHAR_LENGTH);
                entity.Property(e => e.ProviderKey).HasMaxLength(MAX_HALF_VARCHAR_LENGTH);
                entity.Property(e => e.UserId).IsRequired();
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
                entity.HasIndex(e => e.UserId);

                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(UserLogin)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.HasIndex(e => e.RoleId);

                entity.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(UserRole)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.Property(e => e.LoginProvider).HasMaxLength(MAX_HALF_VARCHAR_LENGTH);
                entity.Property(e => e.Name).HasMaxLength(MAX_HALF_VARCHAR_LENGTH);
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(UserToken)}s", SCHEMA_NAME);
            });

        }
        
        #endregion

    }
}
