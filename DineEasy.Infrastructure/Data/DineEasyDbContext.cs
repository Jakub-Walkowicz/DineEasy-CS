using DineEasy.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Data;

public class DineEasyDbContext : DbContext
{
    public DineEasyDbContext(DbContextOptions<DineEasyDbContext> options) : base(options)
    { }
    public DbSet<TimeSlot> TimeSlots { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCustomer(modelBuilder);
        ConfigureTable(modelBuilder);
        ConfigureReservation(modelBuilder);
        ConfigureTimeSlot(modelBuilder);
    }

    private void ConfigureCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.PhoneNumber).IsUnique();
        });
    }

    private void ConfigureTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Capacity).IsRequired();
            entity.Property(e => e.Location).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsActive).IsRequired().HasDefaultValue(false);
            entity.Property(e => e.TableNumber).IsRequired();
        });
    }

    private void ConfigureReservation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ReservationDateTime).IsRequired();
            entity.Property(e => e.PartySize).IsRequired();
            entity.Property(e => e.Duration).IsRequired();
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();
            
            entity.Property(e => e.SpecialRequests).HasMaxLength(500);

            entity.ToTable(t => t.HasCheckConstraint("CK_Reservation_PartySize", "PartySize > 0 AND PartySize <= 20"));
            
            entity.HasIndex(e => new { e.TableId, e.ReservationDateTime })
                .IsUnique()
                .HasDatabaseName("IX_Reservations_TableId_DateTime");
            
            entity.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne<Table>()
                .WithMany()
                .HasForeignKey(e => e.TableId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    
    private void ConfigureTimeSlot(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.DayOfWeek).IsRequired();
        });
    }
}