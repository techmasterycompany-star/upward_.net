using Upward.Application.DTOs.Auth;

namespace Upward.Application.Interfaces.IService
{
    public interface IAuthService
    {
        Task<RegisterResponseDto> RegisterCandidateAsync(RegisterCandidateDto dto);
        Task<RegisterResponseDto> RegisterEmployerAsync(RegisterEmployerDto dto);
        Task VerifyEmailAsync(VerifyEmailDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task ForgotPasswordAsync(ForgotPasswordDto dto);
        Task ResetPasswordAsync(ResetPasswordDto dto);
        Task LogoutAsync(string jti, DateTime tokenExpiresAt);
    }
}
