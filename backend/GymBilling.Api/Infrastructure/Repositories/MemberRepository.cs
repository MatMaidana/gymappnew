using GymBilling.Api.Application.Abstractions;
using GymBilling.Api.Domain.Entities;
using GymBilling.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GymBilling.Api.Infrastructure.Repositories;

public class MemberRepository(AppDbContext dbContext) : IMemberRepository
{
    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await dbContext.Members.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Member>> GetAllAsync(bool activeOnly, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Members.AsNoTracking().AsQueryable();
        if (activeOnly)
        {
            query = query.Where(m => m.IsActive);
        }

        return await query.OrderBy(m => m.FullName).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Member>> GetOverdueAsync(DateOnly today, CancellationToken cancellationToken = default)
        => await dbContext.Members.AsNoTracking()
            .Where(m => m.IsActive && m.NextDueDate < today)
            .OrderBy(m => m.NextDueDate)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Member>> GetDueSoonAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken = default)
        => await dbContext.Members.AsNoTracking()
            .Where(m => m.IsActive && m.NextDueDate >= from && m.NextDueDate <= to)
            .OrderBy(m => m.NextDueDate)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(Member member, CancellationToken cancellationToken = default)
        => await dbContext.Members.AddAsync(member, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await dbContext.SaveChangesAsync(cancellationToken);
}
