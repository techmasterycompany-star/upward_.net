using Upwork.Application.DTOs.Common;

namespace Upwork.Application.Interfaces.IService
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetByJobIdAsync(long jobId);
        Task<CommentDto?> GetByIdAsync(long id);
        Task<CommentDto> CreateAsync(long userId, long jobId, CreateCommentDto request);
        Task<CommentDto?> UpdateAsync(long userId, long commentId, UpdateCommentDto request);
        Task<bool> DeleteAsync(long userId, long commentId);
    }
}
