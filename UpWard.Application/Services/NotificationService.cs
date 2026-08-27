using Upward.Application.DTOs.Notifications;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Domain.Entities;
using Upward.Domain.Enums;

namespace Upward.Application.Services
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
            try
            {
                var items = await _repo.GetByUserIdAsync(userId);
                return items.Select(Map);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve notifications for user {userId}.", ex);
            }
        }

        public async Task<IEnumerable<NotificationDto>> GetUnreadAsync(long userId)
        {
            try
            {
                var items = await _repo.GetUnreadByUserIdAsync(userId);
                return items.Select(Map);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve unread notifications for user {userId}.", ex);
            }
        }

        public async Task<int> GetUnreadCountAsync(long userId)
        {
            try
            {
                return await _repo.GetUnreadCountAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get unread notification count for user {userId}.", ex);
            }
        }

        public async Task<bool> MarkAsReadAsync(long userId, long notificationId)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception($"Failed to mark notification {notificationId} as read.", ex);
            }
        }

        public async Task MarkAllAsReadAsync(long userId)
        {
            try
            {
                await _repo.MarkAllAsReadAsync(userId);
                await _repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to mark all notifications as read for user {userId}.", ex);
            }
        }

        public async Task SendAsync(long recipientId, NotificationType type, string title, string content, string? data = null)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception($"Failed to send notification of type '{type}' to user {recipientId}.", ex);
            }
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
