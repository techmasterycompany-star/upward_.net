using Upwork.Domain.Entities;

namespace Upwork.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
    }
}
