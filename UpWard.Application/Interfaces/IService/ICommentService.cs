using Upward.Application.DTOs.Common;

namespace Upward.Application.Interfaces.IService
{
    public interface ICommentService
    {
        Task<CommentDto> CreateAsync(long userId, long jobId, CreateCommentDto request);
        Task<CommentDto?> UpdateAsync(long userId, long commentId, UpdateCommentDto request);
        Task<bool> DeleteAsync(long userId, long commentId);
        Task<List<CommentDto>> GetByJobIdAsync(long jobId);
    }
}
