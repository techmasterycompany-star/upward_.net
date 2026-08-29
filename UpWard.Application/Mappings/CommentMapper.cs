using Upwork.Application.DTOs.Common;
using Upwork.Domain.Entities;

namespace Upwork.Application.Mappings
{
    public static class CommentMapper
    {
        public static CommentDto ToDto(this Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                JobId = comment.JobId,
                UserId = comment.UserId,
                Content = comment.Content,
                IsApproved = comment.IsApproved,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };
        }

    }
}
