using GymBilling.Api.Application.Abstractions;
using GymBilling.Api.Domain.Entities;
using GymBilling.Api.Infrastructure.Data;

namespace GymBilling.Api.Infrastructure.Repositories;

public class PaymentRepository(AppDbContext dbContext) : IPaymentRepository
{
    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
        => await dbContext.Payments.AddAsync(payment, cancellationToken);
}
