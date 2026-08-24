using Upward.Application.DTOs.Common;
using Upward.Domain.Entities;

namespace Upward.Application.Mappings
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
