namespace Upwork.Application.DTOs.Admin
{
    public class AdminCommentDto
    {
        public long Id { get; set; }
        public string Content { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public long JobId { get; set; }
        public string JobTitle { get; set; } = null!;
        public bool IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
