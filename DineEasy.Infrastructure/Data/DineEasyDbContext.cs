using DineEasy.Domain.Entities;
using DineEasy.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DineEasy.Infrastructure.Data;

public class DineEasyDbContext(DbContextOptions<DineEasyDbContext> options) : DbContext(options)
{
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<TimeSlot> TimeSlots { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        ConfigureTable(modelBuilder);
        ConfigureReservation(modelBuilder);
        ConfigureTimeSlot(modelBuilder);
        ConfigureUser(modelBuilder);
        ConfigureUserProfile(modelBuilder);
        
        SeedData(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@dineeasy.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.Admin,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<UserProfile>().HasData(
            new UserProfile
            {
                Id = 1,
                UserId = 1, // Foreign key reference
                FirstName = "Adam",
                LastName = "Kowalski",
                PhoneNumber = "0123456789",
                DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
        
        modelBuilder.Entity<Table>().HasData(
            new Table { Id = 1, TableNumber = 1, Capacity = 2, IsActive = true },
            new Table { Id = 2, TableNumber = 2, Capacity = 4, IsActive = true }
        );
        
        modelBuilder.Entity<TimeSlot>().HasData(
            new TimeSlot { Id = 1, StartTime = new TimeOnly(18, 0), EndTime = new TimeOnly(19, 0), DayOfWeek = DayOfWeek.Monday },
            new TimeSlot { Id = 2, StartTime = new TimeOnly(19, 0), EndTime = new TimeOnly(20, 0), DayOfWeek = DayOfWeek.Monday }
        );
    }
    
    private void ConfigureUserProfile(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
    
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
    
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
    
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);
    
            entity.Property(e => e.UserId)
                .IsRequired();
            
            entity.HasOne(up => up.User)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        
            entity.HasIndex(e => e.UserId)
                .IsUnique()
                .HasDatabaseName("IX_UserProfiles_UserId");
        });
    }
    
    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
        
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(e => e.Role)
                .IsRequired()
                .HasConversion<string>()  
                .HasDefaultValue(UserRole.User);
            
            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')");
            
            entity.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            entity.HasIndex(e => e.Username)
                .IsUnique()
                .HasDatabaseName("IX_Users_Username");
        });
    }
    private void ConfigureTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table>(entity =>
        {
            entity.ToTable("Tables", t => 
                t.HasCheckConstraint("CK_Table_Capacity", "Capacity > 0 AND Capacity <= 20"));
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Capacity)
                .IsRequired();
            
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            
            entity.Property(e => e.TableNumber)
                .IsRequired();

            entity.HasIndex(e => e.TableNumber)
                .IsUnique()
                .HasDatabaseName("IX_Tables_Number");
        });
    }

    private void ConfigureReservation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.ToTable("Reservations", t =>
            {
                t.HasCheckConstraint("CK_Reservation_PartySize", "PartySize > 0 AND PartySize <= 20");
                t.HasCheckConstraint("CK_Reservation_Duration", "Duration > 0 AND Duration <= 5");
            });
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.ReservationDateTime)
                .IsRequired();
            
            entity.Property(e => e.PartySize)
                .IsRequired();
            
            entity.Property(e => e.Duration)
                .IsRequired();
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<string>();
            
            entity.Property(e => e.SpecialRequests)
                .HasMaxLength(500);
            
            entity.HasIndex(e => new { e.TableId, e.ReservationDateTime })
                .IsUnique()
                .HasDatabaseName("IX_Reservations_TableId_DateTime");
            
            entity.HasIndex(e => e.UserId)
                .HasDatabaseName("IX_Reservations_UserId");
            
            entity.HasOne<User>(r => r.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne<Table>(r => r.Table)
                .WithMany()
                .HasForeignKey(e => e.TableId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    
    private void ConfigureTimeSlot(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TimeSlot>(entity =>
        {
            entity.ToTable("TimeSlots", t => 
            {
                t.HasCheckConstraint("CK_TimeSlot_EndTimeAfterStartTime", 
                    "EndTime > StartTime");
            });
            
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.StartTime)
                .IsRequired();
            
            entity.Property(e => e.EndTime)
                .IsRequired();
            
            entity.Property(e => e.DayOfWeek)
                .IsRequired();
            
            entity.HasIndex(e => new { e.StartTime, e.EndTime, e.DayOfWeek })
                .IsUnique()
                .HasDatabaseName("IX_TimeSlots_StartTime_EndTime_DayOfWeek");
        });
    }
}