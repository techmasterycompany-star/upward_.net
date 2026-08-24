using Upward.Domain.Entities;

namespace Upward.Application.Interfaces.IRepo
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(long commentId);
        Task<List<Comment>> GetByJobIdAsync(long jobId);
        Task AddAsync(Comment comment);
        void Update(Comment comment);
        void Remove(Comment comment);
        Task SaveChangesAsync();
    }
}
