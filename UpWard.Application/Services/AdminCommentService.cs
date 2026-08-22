using Upward.Application.DTOs.Admin;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;

namespace Upward.Application.Services
{
    public class AdminCommentService : IAdminCommentService
    {
        private readonly ICommentRepository repo;
        public AdminCommentService(ICommentRepository repo) => this.repo = repo;



        public async Task<bool> DeleteCommentAsync(long id)
        {
            var comment = await repo.GetCommentByIdAsync(id);
            if (comment is null) return false;
            await repo.DeleteCommentAsync(comment);
            return true;
        }

        public async Task<List<AdminCommentDto>> GetCommentsAsync()
        {
            var comments = await repo.GetAllCommentsAsync();
            return comments.Select(c => new AdminCommentDto
            {
                Id = c.Id,
                Content = c.Content,
                AuthorName = c.User.Name,
                JobId = c.JobId,
                JobTitle = c.Job.Title,
                IsApproved = c.IsApproved,
                CreatedAt = c.CreatedAt
            }).ToList();
        }

        public async Task<bool> HideCommentAsync(long id)
        {
            var comment = await repo.GetCommentByIdAsync(id);
            if (comment is null || !comment.IsApproved) return false;
            await repo.HideCommentAsync(comment);
            return true;
        }

        public async Task<bool> RestoreCommentAsync(long id)
        {
            var comment = await repo.GetCommentByIdAsync(id);
            if (comment is null || comment.IsApproved) return false;
            await repo.RestoreCommentAsync(comment);
            return true;
        }
    }
}
