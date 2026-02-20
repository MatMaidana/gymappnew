using GymBilling.Api.Domain.Enums;

namespace GymBilling.Api.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid MemberId { get; set; }
    public decimal Amount { get; set; }
    public DateTimeOffset PaidAt { get; set; } = DateTimeOffset.UtcNow;
    public PaymentMethod Method { get; set; }
    public string Period { get; set; } = string.Empty;

    public Member? Member { get; set; }
}
