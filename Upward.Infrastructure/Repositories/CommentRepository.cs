using Microsoft.EntityFrameworkCore;
using Upward.Application.Interfaces.IRepo;
using Upward.Domain.Entities;
using Upward.Infrastructure.Data;

namespace Upward.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDBContext context;
        public CommentRepository(AppDBContext context) => this.context = context;
        


        public async Task DeleteCommentAsync(Comment comment)
        {
            comment.IsDeleted = true;
            comment.DeletedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }

        public async Task<List<Comment>> GetAllCommentsAsync() =>
            await context.Comments
               .Include(c => c.User)
               .Include(c => c.Job)
               .Where(c => !c.IsDeleted)
               .OrderByDescending(c => c.CreatedAt)
               .ToListAsync();

        public async Task<Comment?> GetCommentByIdAsync(long id) =>
            await context.Comments
             .Include(c => c.User)
            .Include(c => c.Job)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        public async Task HideCommentAsync(Comment comment)
        {
            comment.IsApproved = false;
            await context.SaveChangesAsync();
        }

        public async Task RestoreCommentAsync(Comment comment)
        {
            comment.IsApproved = true;
            await context.SaveChangesAsync();

        }
    }
}
