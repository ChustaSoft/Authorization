﻿using ChustaSoft.Tools.Authorization.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ChustaSoft.Tools.Authorization
{
    public abstract class AuthorizationContextBase<TUser, TRole> 
            : IdentityDbContext<TUser, TRole, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
        where TUser : User, new()
        where TRole : Role
    {
        
        #region Constants

        private const string SCHEMA_NAME = "Auth";
        private const int MAX_FULL_VARCHAR_LENGTH = 256;
        private const int MAX_HALF_VARCHAR_LENGTH = 128;

        #endregion


        #region Constructor

        public AuthorizationContextBase(DbContextOptions options) 
            : base(options) 
        { }


        #endregion


        #region Protected methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
                entity.Property(e => e.Name).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.Property(e => e.NormalizedName).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Name).IsUnique();
#if NETCOREAPP3_1
                entity.HasIndex(e => e.NormalizedName).IsUnique().HasName("RoleNameIndex");
#else
                entity.HasIndex(e => e.NormalizedName).IsUnique().HasDatabaseName("RoleNameIndex");
#endif

                entity.ToTable($"{nameof(Role)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.RoleId).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.RoleId);

                entity.HasOne<TRole>().WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(RoleClaim)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<TUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
                entity.Property(e => e.UserName).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.Property(e => e.NormalizedEmail).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();
                entity.Property(e => e.NormalizedUserName).HasMaxLength(MAX_FULL_VARCHAR_LENGTH).IsRequired();

                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.RegistrationDate).HasDefaultValueSql("sysdatetimeoffset()");

                entity.HasKey(e => e.Id);

#if NETCOREAPP3_1
                entity.HasIndex(e => e.NormalizedEmail).IsUnique().HasName("EmailIndex");
                entity.HasIndex(e => e.NormalizedUserName).IsUnique().HasName("UserNameIndex");
                entity.HasIndex(e => e.PhoneNumber).IsUnique().HasName("PhoneNumberIndex");
#else
                entity.HasIndex(e => e.NormalizedEmail).IsUnique().HasDatabaseName("EmailIndex");
                entity.HasIndex(e => e.NormalizedUserName).IsUnique().HasDatabaseName("UserNameIndex");
                entity.HasIndex(e => e.PhoneNumber).IsUnique().HasDatabaseName("PhoneNumberIndex");
#endif
                entity.ToTable($"{nameof(User)}s", SCHEMA_NAME);

            });


            modelBuilder.Entity<UserClaim>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).IsRequired();
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.UserId);

                entity.HasOne<TUser>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(UserClaim)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.Property(e => e.LoginProvider).HasMaxLength(MAX_HALF_VARCHAR_LENGTH);
                entity.Property(e => e.ProviderKey).HasMaxLength(MAX_HALF_VARCHAR_LENGTH);
                entity.Property(e => e.UserId).IsRequired();
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });
                entity.HasIndex(e => e.UserId);

                entity.HasOne<TUser>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(UserLogin)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });
                entity.HasIndex(e => e.RoleId);

                entity.HasOne<TRole>().WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<TUser>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(UserRole)}s", SCHEMA_NAME);
            });

            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.Property(e => e.LoginProvider).HasMaxLength(MAX_HALF_VARCHAR_LENGTH);
                entity.Property(e => e.Name).HasMaxLength(MAX_HALF_VARCHAR_LENGTH);
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne<TUser>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);

                entity.ToTable($"{nameof(UserToken)}s", SCHEMA_NAME);
            });
        }

#endregion

    }



    public class AuthorizationContext : AuthorizationContextBase<User, Role>
    {

        public AuthorizationContext(DbContextOptions<AuthorizationContext> options)
           : base(options)
        { }

    }

}
