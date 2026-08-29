using Upwork.Domain.Enums;

namespace Upwork.Application.DTOs.Notifications
{
    public class NotificationDto
    {
        public long Id { get; set; }
        public NotificationType Type { get; set; }
        public string TypeName => Type.ToString();
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? Data { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
