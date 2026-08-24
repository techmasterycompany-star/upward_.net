namespace Upward.Application.DTOs.Common
{
    public class CommentDto
    {
        public long Id { get; set; }
        public long JobId { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string Content { get; set; } = null!;
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }


}
