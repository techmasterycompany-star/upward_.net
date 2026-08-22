using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IRepo
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllCommentsAsync();
        Task<Comment?> GetCommentByIdAsync(long id);
        Task HideCommentAsync(Comment comment);
        Task RestoreCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);

    }
}
