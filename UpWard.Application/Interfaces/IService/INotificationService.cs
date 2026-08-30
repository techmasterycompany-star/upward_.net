using Upwork.Application.DTOs.Notifications;
using Upwork.Domain.Enums;

namespace Upwork.Application.Interfaces.IService
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetAllAsync(long userId);
        Task<IEnumerable<NotificationDto>> GetUnreadAsync(long userId);
        Task<int> GetUnreadCountAsync(long userId);
        Task<bool> MarkAsReadAsync(long userId, long notificationId);
        Task MarkAllAsReadAsync(long userId);

        Task SendAsync(long userId, NotificationType type, string title, string content, string? data = null);

        // Candidate 
        Task NotifyApplicationSubmittedAsync(long candidateUserId, string jobTitle);
        Task NotifyApplicationAcceptedAsync(long candidateUserId, string jobTitle);
        Task NotifyApplicationRejectedAsync(long candidateUserId, string jobTitle, string? reason = null);
        Task NotifyApplicationStatusChangedAsync(long candidateUserId, string jobTitle, string newStatus);

        // Employer 
        Task NotifyNewApplicationReceivedAsync(long employerUserId, string jobTitle, string candidateName);
        Task NotifyJobApprovedAsync(long employerUserId, string jobTitle);
        Task NotifyJobRejectedAsync(long employerUserId, string jobTitle, string? reason = null);
        Task NotifyJobDeadlineApproachingAsync(long employerUserId, string jobTitle, DateTime deadline);
        Task NotifyJobExpiredAsync(long employerUserId, string jobTitle);

        // Admin 
        Task NotifyNewJobPendingApprovalAsync(long adminUserId, string jobTitle, string employerName);
    }
}
