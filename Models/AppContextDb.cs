using Microsoft.EntityFrameworkCore;
using CarManagmentSystem.Models;
using CarManagmentSystem;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<ServiceHistory> ServiceHistory { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Manager> Manager { get; set; }
    public DbSet<RentalHistory> RentalHistory { get; set; }
    public DbSet<BuyerHistory> BuyerHistory { get; set; }
    public DbSet<CarDamageHistory> CarDamageHistory { get; set; }
    public DbSet<AccidentHistory> AccidentHistory { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Branch> Branch { get; set; }
    public DbSet<Brand> Brand { get; set; }
    public DbSet<Car> Car { get; set; }
    public DbSet<Client> Client { get; set; }
    public DbSet<CarImage> CarImage { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Define primary keys
        modelBuilder.Entity<ServiceHistory>()
            .HasKey(s => s.ServiceId);

        modelBuilder.Entity<Employee>()
            .HasKey(e => e.UserId);

        modelBuilder.Entity<Manager>()
            .HasKey(m => m.UserId);

        modelBuilder.Entity<RentalHistory>()
            .HasKey(r => r.RentalId);

        modelBuilder.Entity<BuyerHistory>()
            .HasKey(b => b.HistoryId);

        modelBuilder.Entity<CarDamageHistory>()
            .HasKey(d => d.DamageId);

        modelBuilder.Entity<AccidentHistory>()
            .HasKey(a => a.AccidentId);

        modelBuilder.Entity<Branch>()
            .HasKey(b => b.BranchId);

        modelBuilder.Entity<Brand>()
            .HasKey(b => b.BrandId);

        modelBuilder.Entity<Car>()
            .HasKey(c => c.CarId);

        modelBuilder.Entity<Client>()
            .HasKey(c => c.UserId);

        modelBuilder.Entity<CarImage>()
            .HasKey(i => i.ImageId);

        // Define relationships
        modelBuilder.Entity<ServiceHistory>()
            .HasOne(s => s.Car)
            .WithMany(c => c.ServiceHistories)
            .HasForeignKey(s => s.CarId);

        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Branch)
            .WithMany(b => b.Employees)
            .HasForeignKey(e => e.BranchId);

        modelBuilder.Entity<Manager>()
            .HasOne(m => m.Branch)
            .WithMany(b => b.Managers)
            .HasForeignKey(m => m.BranchId);

        modelBuilder.Entity<RentalHistory>()
            .HasOne(r => r.Car)
            .WithMany(c => c.RentalHistories)
            .HasForeignKey(r => r.CarId);

        modelBuilder.Entity<RentalHistory>()
            .HasOne(r => r.Client)
            .WithMany(c => c.RentalHistories)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<BuyerHistory>()
            .HasOne(b => b.Car)
            .WithMany(c => c.BuyerHistories)
            .HasForeignKey(b => b.CarId);

        modelBuilder.Entity<BuyerHistory>()
            .HasOne(b => b.Client)
            .WithMany(c => c.BuyerHistories)
            .HasForeignKey(b => b.UserId);

        modelBuilder.Entity<CarDamageHistory>()
            .HasOne(d => d.Car)
            .WithMany(c => c.CarDamageHistories)
            .HasForeignKey(d => d.CarId);

        modelBuilder.Entity<CarDamageHistory>()
            .HasOne(d => d.Client)
            .WithMany(c => c.CarDamageHistories)
            .HasForeignKey(d => d.UserId);

        modelBuilder.Entity<AccidentHistory>()
            .HasOne(a => a.Car)
            .WithMany(c => c.AccidentHistories)
            .HasForeignKey(a => a.CarId);

        modelBuilder.Entity<AccidentHistory>()
            .HasOne(a => a.Client)
            .WithMany(c => c.AccidentHistories)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<Car>()
            .HasOne(c => c.Brand)
            .WithMany(b => b.Cars)
            .HasForeignKey(c => c.BrandId);

        modelBuilder.Entity<CarImage>()
            .HasOne(i => i.Car)
            .WithMany(c => c.CarImages)
            .HasForeignKey(i => i.CarId);

        base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>()
        .Property(u => u.UserRole)
        .HasConversion(
            v => v.ToString(),
            v => (UserRole)Enum.Parse(typeof(UserRole), v));    
    }
}
