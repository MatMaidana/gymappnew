using GymBilling.Api.Domain.Entities;

namespace GymBilling.Api.Application.Abstractions;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> GetAllAsync(bool activeOnly, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> GetOverdueAsync(DateOnly today, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Member>> GetDueSoonAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default);
    Task AddAsync(Member member, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
