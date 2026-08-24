using Upward.Application.DTOs.Common;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Application.Mappings;
using Upward.Domain.Entities;

namespace Upward.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentDto> CreateAsync(long userId, long jobId, CreateCommentDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                throw new ArgumentException("Comment content is required.");

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

        public async Task<List<CommentDto>> GetByJobIdAsync(long jobId)
        {
            var comments = await _commentRepository.GetByJobIdAsync(jobId);

            return comments.Select(c => c.ToDto()).ToList();
        }

    }
}
