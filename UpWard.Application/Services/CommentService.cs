using Upwork.Application.DTOs.Common;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces.IService;
using Upwork.Application.Mappings;
using Upwork.Domain.Entities;

namespace Upwork.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IJobRepository _jobRepository; 

        public CommentService(ICommentRepository commentRepository, IJobRepository jobRepository)
        {
            _commentRepository = commentRepository;
            _jobRepository = jobRepository;
        }

        public async Task<List<CommentDto>> GetByJobIdAsync(long jobId)
        {
            var comments = await _commentRepository.GetByJobIdAsync(jobId);

            return comments.Select(c => c.ToDto()).ToList();
        }

        public async Task<CommentDto?> GetByIdAsync(long id)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(id);
            return comment?.ToDto();
        }

        public async Task<CommentDto> CreateAsync(long userId, long jobId, CreateCommentDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                throw new ArgumentException("Comment content is required.");

            var job = await _jobRepository.GetApprovedJobByIdAsync(jobId);

            if (job == null)
                throw new KeyNotFoundException("Job not found");

            var comment = new Comment
            {
                JobId = jobId,
                UserId = userId,
                Content = request.Content.Trim(),
                IsApproved = true
            };

            await _commentRepository.AddAsync(comment);
            await _commentRepository.SaveChangesAsync();

            return comment.ToDto();
        }

        public async Task<CommentDto?> UpdateAsync(long userId, long commentId, UpdateCommentDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                throw new ArgumentException("Comment content is required.");

            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            if (comment is null)
                return null;

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("You can only edit your own comments.");

            comment.Content = request.Content.Trim();
            comment.UpdatedAt = DateTime.UtcNow;

            _commentRepository.Update(comment);
            await _commentRepository.SaveChangesAsync();

            return comment.ToDto();
        }

        public async Task<bool> DeleteAsync(long userId, long commentId)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(commentId);

            if (comment is null)
                return false;

            if (comment.UserId != userId)
                throw new UnauthorizedAccessException("You can only delete your own comments.");

            _commentRepository.Remove(comment);
            await _commentRepository.SaveChangesAsync();

            return true;
        }

    }
}
