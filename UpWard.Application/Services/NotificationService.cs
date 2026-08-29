using Upwork.Application.DTOs.Notifications;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Domain.Entities;
using Upwork.Domain.Enums;

namespace Upwork.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;

        public NotificationService(INotificationRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllAsync(long userId)
        {
            var items = await _repo.GetByUserIdAsync(userId);
                
            if(items == null)
            {
                return Enumerable.Empty<NotificationDto>();
            }

            return items.Select(Map);
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadAsync(long userId)
        {
            var items = await _repo.GetUnreadByUserIdAsync(userId);
            if(items == null)
            {
                return Enumerable.Empty<NotificationDto>();
            }
            return items.Select(Map);
        }

        public async Task<int> GetUnreadCountAsync(long userId)
        {
               return await _repo.GetUnreadCountAsync(userId);
        }

        public async Task<bool> MarkAsReadAsync(long userId, long notificationId)
        {
                var notification = await _repo.GetByIdAsync(notificationId);

                if (notification is null || notification.UserId != userId)
                    return false;

                if (notification.IsRead)
                    return true;

                await _repo.MarkAsReadAsync(notificationId);
                await _repo.SaveChangesAsync();
                return true;
        }

        public async Task MarkAllAsReadAsync(long userId)
        {
                await _repo.MarkAllAsReadAsync(userId);
                await _repo.SaveChangesAsync();
        }

        public async Task SendAsync(long recipientId, NotificationType type, string title, string content, string? data = null)
        {
                var notification = new Notification
                {
                    UserId = recipientId,
                    Type = type,
                    Title = title,
                    Content = content,
                    Data = data,
                    IsRead = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _repo.AddAsync(notification);
                await _repo.SaveChangesAsync();
        }

        // ── Candidate Notifications

        public Task NotifyApplicationSubmittedAsync(long candidateUserId, string jobTitle)
            => SendAsync(candidateUserId, NotificationType.ApplicationSubmitted,
                "Application Submitted",
                $"Your application for \"{jobTitle}\" has been successfully submitted.");

        public Task NotifyApplicationAcceptedAsync(long candidateUserId, string jobTitle)
            => SendAsync(candidateUserId, NotificationType.ApplicationAccepted,
                "Application Accepted",
                $"Congratulations! Your application for \"{jobTitle}\" has been accepted.");

        public Task NotifyApplicationRejectedAsync(long candidateUserId, string jobTitle, string? reason = null)
        {
            var content = $"Unfortunately, your application for \"{jobTitle}\" was not selected.";
            if (!string.IsNullOrWhiteSpace(reason))
                content += $" Reason: {reason}";

            return SendAsync(candidateUserId, NotificationType.ApplicationRejected, "Application Rejected", content);
        }

        public Task NotifyApplicationStatusChangedAsync(long candidateUserId, string jobTitle, string newStatus)
            => SendAsync(candidateUserId, NotificationType.ApplicationStatusChanged,
                "Application Status Updated",
                $"The status of your application for \"{jobTitle}\" has changed to: {newStatus}.");

        // ── Employer Notifications

        public Task NotifyNewApplicationReceivedAsync(long employerUserId, string jobTitle, string candidateName)
            => SendAsync(employerUserId, NotificationType.NewApplicationReceived,
                "New Application Received",
                $"{candidateName} has applied for your job listing \"{jobTitle}\".");

        public Task NotifyJobApprovedAsync(long employerUserId, string jobTitle)
            => SendAsync(employerUserId, NotificationType.JobApproved,
                "Job Listing Approved",
                $"Your job listing \"{jobTitle}\" has been approved and is now publicly visible.");

        public Task NotifyJobRejectedAsync(long employerUserId, string jobTitle, string? reason = null)
        {
            var content = $"Your job listing \"{jobTitle}\" was rejected by the admin.";
            if (!string.IsNullOrWhiteSpace(reason))
                content += $" Reason: {reason}";

            return SendAsync(employerUserId, NotificationType.JobRejected, "Job Listing Rejected", content);
        }

        public Task NotifyJobDeadlineApproachingAsync(long employerUserId, string jobTitle, DateTime deadline)
            => SendAsync(employerUserId, NotificationType.JobDeadlineApproaching,
                "Job Deadline Approaching",
                $"Your job listing \"{jobTitle}\" expires on {deadline:yyyy-MM-dd}. Consider extending the deadline.");

        public Task NotifyJobExpiredAsync(long employerUserId, string jobTitle)
            => SendAsync(employerUserId, NotificationType.JobExpired,
                "Job Listing Expired",
                $"Your job listing \"{jobTitle}\" has expired and is no longer visible to candidates.");

        // ── Admin Notifications

        public Task NotifyNewJobPendingApprovalAsync(long adminUserId, string jobTitle, string employerName)
            => SendAsync(adminUserId, NotificationType.NewJobPendingApproval,
                "New Job Awaiting Approval",
                $"\"{jobTitle}\" posted by {employerName} is waiting for your review.");

        // ── Mapping helper

        private static NotificationDto Map(Notification n) => new()
        {
            Id = n.Id,
            Type = n.Type,
            Title = n.Title,
            Content = n.Content,
            Data = n.Data,
            IsRead = n.IsRead,
            ReadAt = n.ReadAt,
            CreatedAt = n.CreatedAt
        };
    }
}
