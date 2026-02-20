using GymBilling.Api.Domain.Entities;

namespace GymBilling.Api.Application.Abstractions;

public interface IPaymentRepository
{
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
}
