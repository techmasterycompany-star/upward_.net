using Upward.Application.DTOs.Admin;

namespace Upward.Application.Interfaces.IService
{
    public interface IAdminCommentService
    {
        Task<List<AdminCommentDto>> GetCommentsAsync();
        Task<bool> HideCommentAsync(long id);
        Task<bool> RestoreCommentAsync(long id);
        Task<bool> DeleteCommentAsync(long id);

    }
}
