using Microsoft.EntityFrameworkCore;
using System;

namespace OTUS_CI.Data
{
    public class EfDbContext(DbContextOptions<EfDbContext> options) : DbContext(options)
    {
        public DbSet<MyUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
