using GymBilling.Api.Domain.Enums;

namespace GymBilling.Api.Api.Contracts;

public record CreateMemberRequest(
    string FullName,
    string Phone,
    decimal MonthlyFee,
    DateOnly NextDueDate,
    string? Notes);

public record UpdateMemberRequest(
    string FullName,
    string Phone,
    decimal MonthlyFee,
    DateOnly NextDueDate,
    bool IsActive,
    string? Notes);

public record MemberResponse(
    Guid Id,
    string FullName,
    string Phone,
    decimal MonthlyFee,
    DateOnly NextDueDate,
    bool IsActive,
    string? Notes);

public record MarkPaymentRequest(
    decimal Amount,
    DateTimeOffset PaidAt,
    PaymentMethod Method,
    string Period);

public record PaymentResponse(
    Guid Id,
    Guid MemberId,
    decimal Amount,
    DateTimeOffset PaidAt,
    PaymentMethod Method,
    string Period,
    DateOnly NewNextDueDate);
