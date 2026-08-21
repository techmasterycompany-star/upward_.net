using Upward.Domain.Entities;

namespace Upward.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
    }
}
