using GymBilling.Api.Api.Contracts;
using GymBilling.Api.Application.Abstractions;
using GymBilling.Api.Domain.Entities;
using GymBilling.Api.Domain.Enums;

namespace GymBilling.Api.Application.Services;

public class MemberService(IMemberRepository memberRepository, IPaymentRepository paymentRepository)
{
    public async Task<IReadOnlyList<MemberResponse>> ListAsync(string? filter, int dueSoonDays, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        return filter?.ToLowerInvariant() switch
        {
            "overdue" => (await memberRepository.GetOverdueAsync(today, cancellationToken)).Select(ToResponse).ToList(),
            "duesoon" => (await memberRepository.GetDueSoonAsync(today, today.AddDays(dueSoonDays), cancellationToken)).Select(ToResponse).ToList(),
            _ => (await memberRepository.GetAllAsync(activeOnly: false, cancellationToken)).Select(ToResponse).ToList()
        };
    }

    public async Task<MemberResponse?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(id, cancellationToken);
        return member is null ? null : ToResponse(member);
    }

    public async Task<MemberResponse> CreateAsync(CreateMemberRequest request, CancellationToken cancellationToken)
    {
        var member = new Member
        {
            FullName = request.FullName.Trim(),
            Phone = request.Phone.Trim(),
            MonthlyFee = request.MonthlyFee,
            NextDueDate = request.NextDueDate,
            Notes = request.Notes,
            IsActive = true
        };

        await memberRepository.AddAsync(member, cancellationToken);
        await memberRepository.SaveChangesAsync(cancellationToken);
        return ToResponse(member);
    }

    public async Task<MemberResponse?> UpdateAsync(Guid id, UpdateMemberRequest request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(id, cancellationToken);
        if (member is null)
        {
            return null;
        }

        member.FullName = request.FullName.Trim();
        member.Phone = request.Phone.Trim();
        member.MonthlyFee = request.MonthlyFee;
        member.NextDueDate = request.NextDueDate;
        member.IsActive = request.IsActive;
        member.Notes = request.Notes;

        await memberRepository.SaveChangesAsync(cancellationToken);
        return ToResponse(member);
    }

    public async Task<bool> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(id, cancellationToken);
        if (member is null)
        {
            return false;
        }

        member.IsActive = false;
        await memberRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<PaymentResponse?> MarkPaymentAsync(Guid memberId, MarkPaymentRequest request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetByIdAsync(memberId, cancellationToken);
        if (member is null)
        {
            return null;
        }

        var payment = new Payment
        {
            MemberId = memberId,
            Amount = request.Amount,
            Method = request.Method,
            Period = request.Period,
            PaidAt = request.PaidAt
        };

        await paymentRepository.AddAsync(payment, cancellationToken);
        member.NextDueDate = member.NextDueDate.AddMonths(1);
        await memberRepository.SaveChangesAsync(cancellationToken);

        return new PaymentResponse(payment.Id, payment.MemberId, payment.Amount, payment.PaidAt, payment.Method, payment.Period, member.NextDueDate);
    }

    private static MemberResponse ToResponse(Member member)
        => new(member.Id, member.FullName, member.Phone, member.MonthlyFee, member.NextDueDate, member.IsActive, member.Notes);
}
