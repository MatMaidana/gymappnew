using GymBilling.Api.Domain.Entities;
using GymBilling.Api.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymBilling.Api.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.FullName).HasMaxLength(140).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30).IsRequired();
            entity.Property(x => x.MonthlyFee).HasPrecision(10, 2);
            entity.Property(x => x.Notes).HasMaxLength(500);
            entity.HasMany(x => x.Payments)
                .WithOne(x => x.Member)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Amount).HasPrecision(10, 2);
            entity.Property(x => x.Period).HasMaxLength(7).IsRequired();
            entity.Property(x => x.Method)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();
        });

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        var memberId = Guid.Parse("f5f503c6-258f-45d3-a8f2-8fe06b8d0b1d");
        modelBuilder.Entity<Member>().HasData(new Member
        {
            Id = memberId,
            FullName = "Sofía Fernández",
            Phone = "5491122334455",
            MonthlyFee = 20000,
            NextDueDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-2)),
            IsActive = true,
            Notes = "Prefiere pago por transferencia"
        });

        modelBuilder.Entity<Payment>().HasData(new Payment
        {
            Id = Guid.Parse("06c52211-df8a-4871-b0ef-2dd59dc9eacd"),
            MemberId = memberId,
            Amount = 20000,
            Method = PaymentMethod.Transfer,
            Period = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(-1)).ToString("yyyy-MM"),
            PaidAt = DateTimeOffset.UtcNow.AddMonths(-1)
        });
    }
}
