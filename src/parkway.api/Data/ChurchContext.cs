using Microsoft.EntityFrameworkCore;
using ChurchAccountingSystem.Models;

namespace ChurchAccountingSystem.Data
{
    public class ChurchContext : DbContext
    {
        public ChurchContext(DbContextOptions<ChurchContext> options) : base(options)
        {
        }

        // People and Businesses
        public DbSet<IndividualEntity> Individuals { get; set; }
        public DbSet<BusinessEntity> Businesses { get; set; }

        // Contributions and Funds
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Fund> Funds { get; set; }

        // Optional: Add this if you need raw access to all entities regardless of type
        public DbSet<Entity> Entities { get; set; }

        // Optional: In case you want to manage employee/contractor details separately
        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Support value objects like Address
            modelBuilder.Entity<IndividualEntity>()
                .OwnsOne(e => e.Address);

            modelBuilder.Entity<BusinessEntity>()
                .OwnsOne(e => e.Address);

            // Fund self-referencing relationship
            modelBuilder.Entity<Fund>()
                .HasOne(f => f.ParentFund)
                .WithMany(f => f.SubFunds)
                .HasForeignKey(f => f.ParentFundId);

            // Polymorphic Entity table (TPH strategy)
            modelBuilder.Entity<IndividualEntity>().HasBaseType<Entity>();
            modelBuilder.Entity<BusinessEntity>().HasBaseType<Entity>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
