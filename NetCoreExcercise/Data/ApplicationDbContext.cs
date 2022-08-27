using Microsoft.EntityFrameworkCore;
using NetCoreExercise.Models;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace NetCoreExercise.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CustomerType>().HasData(new CustomerType { Id = 1, Name = "Default CustomerType" });

            builder.Entity<Customer>()
            .Property(b => b.LastUpdated)
                .HasDefaultValueSql("getdate()");
        }
    }
}