using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDBContext _context;

        public CommentRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Comment?> GetByIdAsync(long commentId)
        {
            return await _context.Comments.FirstOrDefaultAsync(x => x.Id == commentId && !x.IsDeleted);
        }

        public async Task<List<Comment>> GetByJobIdAsync(long jobId)
        {
            return await _context.Comments
                .Where(x => x.JobId == jobId && !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
        }

        public void Remove(Comment comment)
        {
            comment.IsDeleted = true;
            comment.DeletedAt = DateTime.UtcNow;
            _context.Comments.Update(comment);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


}
