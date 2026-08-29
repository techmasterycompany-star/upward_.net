using Microsoft.EntityFrameworkCore;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Domain.Entities;
using Upwork.Infrastructure.Data;

namespace Upwork.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDBContext _context;

        public CommentRepository(AppDBContext context)
        {
            _context = context;
        }

        // Admin
        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Job)
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Comment?> GetCommentForAdminAsync(long id)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Job)
                .FirstOrDefaultAsync(c =>
                    c.Id == id &&
                    !c.IsDeleted);
        }

        public async Task HideCommentAsync(Comment comment)
        {
            comment.IsApproved = false;

            await _context.SaveChangesAsync();
        }

        public async Task RestoreCommentAsync(Comment comment)
        {
            comment.IsApproved = true;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            comment.IsDeleted = true;
            comment.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // Comments
        public async Task<Comment?> GetCommentByIdAsync(long commentId)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(x =>
                    x.Id == commentId &&
                    !x.IsDeleted);
        }

        public async Task<List<Comment>> GetByJobIdAsync(long jobId)
        {
            return await _context.Comments
                .Where(x =>
                    x.JobId == jobId &&
                    !x.IsDeleted)
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