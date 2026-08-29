using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IRepo
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetByUserIdAsync(long userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(long userId);
        Task<int> GetUnreadCountAsync(long userId);
        Task<Notification?> GetByIdAsync(long id);
        Task AddAsync(Notification notification);
        Task MarkAsReadAsync(long notificationId);
        Task MarkAllAsReadAsync(long userId);
        Task<int> SaveChangesAsync();
    }
}
