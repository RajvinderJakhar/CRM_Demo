using CRM_Auth.DataAccess.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_Auth.DataAccess
{
    public class CRM_AuthDBContext : IdentityDbContext<Accounts, AppRoles, long, Account_Claims, Account_Roles, Account_Logins, Account_RoleClaims, Account_Tokens>
    {
        public CRM_AuthDBContext(DbContextOptions<CRM_AuthDBContext> options)
            : base(options) { }


        /// <summary>
        /// Creating Identity user custom entities
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Accounts>(b =>
            {
                b.ToTable("Accounts");

                // Each Account can have many Account_Claims
                b.HasMany(e => e.Account_Claims)
                    .WithOne(e => e.Accounts)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each Account can have many Account_Logins
                b.HasMany(e => e.Account_Logins)
                    .WithOne(e => e.Accounts)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each Account can have many Account_Tokens
                b.HasMany(e => e.Account_Tokens)
                    .WithOne(e => e.Accounts)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each Account can have many entries in the Account_Roles join table
                b.HasMany(e => e.Account_Roles)
                    .WithOne(e => e.Accounts)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<AppRoles>(b =>
            {
                b.ToTable("AppRoles");

                // Each Role can have many entries in the Account_Roles join table
                b.HasMany(e => e.Account_Roles)
                    .WithOne(e => e.AppRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated Account_RoleClaims
                b.HasMany(e => e.Account_RoleClaims)
                    .WithOne(e => e.AppRoles)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<Account_Roles>(b =>
            {
                b.ToTable("Account_Roles");
            });

            modelBuilder.Entity<Account_Claims>(b =>
            {
                b.ToTable("Account_Claims");
            });

            modelBuilder.Entity<Account_Logins>(b =>
            {
                b.ToTable("Account_Logins");
            });

            modelBuilder.Entity<Account_Tokens>(b =>
            {
                b.ToTable("Account_Tokens");
            });

            modelBuilder.Entity<Account_RoleClaims>(b =>
            {
                b.ToTable("Account_RoleClaims");
            });
        }
    }
}
