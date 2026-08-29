using Upward.Application.Interfaces;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDBContext _context;

        public PaymentRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await SaveChanges();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await SaveChanges();
        }

        private async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
