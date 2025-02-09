using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Repositories
{
    public class EfDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Preference> Preferences { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
        public DbSet<CustomerPreference> CustomerPreferences { get; set; }


        public EfDbContext()
        {
            
        }

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //SEED
            modelBuilder.Entity<Customer>().HasData(FakeDataFactory.Customers);
            modelBuilder.Entity<Employee>().HasData(FakeDataFactory.Employees);
            modelBuilder.Entity<Role>().HasData(FakeDataFactory.Roles);
            modelBuilder.Entity<Preference>().HasData(FakeDataFactory.Preferences);
            modelBuilder.Entity<CustomerPreference>().HasData(FakeDataFactory.CustomerPreferences);

            modelBuilder.Entity<CustomerPreference>()
                .HasKey(x => new { x.CustomerId, x.PreferenceId });

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.CustomerPreferences)
                .HasForeignKey(x => x.CustomerId);

            modelBuilder.Entity<CustomerPreference>()
                .HasOne(x => x.Preference)
                .WithMany(x => x.CustomerPreferences)
                .HasForeignKey(x => x.PreferenceId);

            modelBuilder.Entity<Employee>().Property(x => x.FirstName).HasMaxLength(40).HasDefaultValue("User123");
            modelBuilder.Entity<Employee>().Property(x => x.LastName).HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.Email).HasMaxLength(40);

            modelBuilder.Entity<Role>().Property(x => x.Name).HasMaxLength(50);
            modelBuilder.Entity<Role>().Property(x => x.Description).HasMaxLength(200);

            modelBuilder.Entity<Customer>().Property(x => x.FirstName).HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(x => x.LastName).HasMaxLength(50);
            modelBuilder.Entity<Customer>().Property(x => x.Email).HasMaxLength(40);

            modelBuilder.Entity<Preference>().Property(x => x.Name).HasMaxLength(50);

            modelBuilder.Entity<PromoCode>().Property(x => x.Code).HasMaxLength(20);
            modelBuilder.Entity<PromoCode>().Property(x => x.ServiceInfo).HasMaxLength(100);
            modelBuilder.Entity<PromoCode>().Property(x => x.PartnerName).HasMaxLength(100);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
                .UseSqlite("Filename=Promocodes.db", options => options.MigrationsAssembly("PromoCodeFactory.DataAccess"))
                .UseLazyLoadingProxies();
        }
    }
}
