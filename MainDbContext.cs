using FixoraBackend.Data;
using FixoraBackend.Models;
using FixoraBackend.Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;


public class MainDbContext : IdentityDbContext<ApplicationUser>
{
    public MainDbContext(DbContextOptions<MainDbContext> options)
        : base(options) { }

    // Business
    public DbSet<BusinessClient> BusinessClients { get; set; }
        public DbSet<BusinessOrder> BusinessOrders { get; set; }
        public DbSet<BusinessSite> BusinessSites { get; set; }

        // Users & Access
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        // Operations
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<WarrantyCase> WarrantyCases { get; set; }

        // Delivery
        public DbSet<DeliveryJob> DeliveryJobs { get; set; }
        public DbSet<Driver> Drivers { get; set; }

        // System
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProviderPerformance> ProviderPerformances { get; set; }
        public DbSet<SystemSetting> Settings { get; set; }
    public DbSet<ProviderLocationHistory> ProviderLocationHistories { get; set; }
    public DbSet<WorkProof> WorkProofs { get; set; }

    public DbSet<Supervisor> Supervisors { get; set; }
    public DbSet<Provider> Providers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        // Apply all entity configurations from EntityConfigurations/ folder
        //  modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<Supervisor>()
            .HasIndex(s => s.Code)
            .IsUnique();

        modelBuilder.Entity<Provider>()
            .HasOne(p => p.Supervisor)
            .WithMany(s => s.Providers)
            .HasForeignKey(p => p.SupervisorId)
            .OnDelete(DeleteBehavior.SetNull);
    
            base.OnModelCreating(modelBuilder);
        }
    }




//using FixoraBackend.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace FixoraBackend.EntityConfigurations
//{
//    public class BusinessClientConfiguration : IEntityTypeConfiguration<BusinessClient>
//    {
//        public void Configure(EntityTypeBuilder<BusinessClient> builder)
//        {
//            builder.HasKey(x => x.Id);
//            builder.Property(x => x.CompanyName).HasMaxLength(100).IsRequired();
//            builder.Property(x => x.ManagerName).HasMaxLength(100).IsRequired();
//            builder.Property(x => x.Phone).HasMaxLength(20).IsRequired();
//            builder.Property(x => x.Region).HasMaxLength(100).IsRequired();
//            builder.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
//        }
//    }
//}

