namespace GymBilling.Api.Domain.Entities;

public class Member
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal MonthlyFee { get; set; }
    public DateOnly NextDueDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public ICollection<Payment> Payments { get; set; } = [];
}
