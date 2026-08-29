using Upwork.Application.DTOs.Admin;

namespace Upwork.Application.Interfaces.IService
{
    public interface IAdminCommentService
    {
        Task<List<AdminCommentDto>> GetCommentsAsync();
        Task<bool> HideCommentAsync(long id);
        Task<bool> RestoreCommentAsync(long id);
        Task<bool> DeleteCommentAsync(long id);

    }
}
