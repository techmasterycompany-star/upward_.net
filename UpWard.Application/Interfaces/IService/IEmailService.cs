namespace Upward.Application.Interfaces.IService
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string toEmail, string userName, string token);
        Task SendPasswordResetEmailAsync(string toEmail, string userName, string token);
    }
}
