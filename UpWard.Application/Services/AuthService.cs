using Upward.Application.DTOs.Auth;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Domain.Entities;
using Upward.Domain.Enums;

namespace Upward.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserAuthRepository _userRepository;
        private readonly IEmailVerificationTokenRepository _verificationTokenRepository;
        private readonly IPasswordResetTokenRepository _resetTokenRepository;
        private readonly IRevokedTokenRepository _revokedTokenRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthService(
            IUserAuthRepository userRepository,
            IEmailVerificationTokenRepository verificationTokenRepository,
            IPasswordResetTokenRepository resetTokenRepository,
            IRevokedTokenRepository revokedTokenRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _verificationTokenRepository = verificationTokenRepository;
            _resetTokenRepository = resetTokenRepository;
            _revokedTokenRepository = revokedTokenRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _emailService = emailService;
        }


        private async Task IssueVerificationEmailAsync(
           User user)
        {
            var token = new EmailVerificationToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            await _verificationTokenRepository.AddAsync(token);

            await _emailService.SendVerificationEmailAsync(
                user.Email,
                user.Name,
                token.Token);
        }

        private AuthResponseDto BuildAuthResponse(
            User user)
        {
            var generated =
                _tokenService.GenerateAccessToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                AccessToken = generated.AccessToken,
                ExpiresAt = generated.ExpiresAt
            };
        }

        public async Task<RegisterResponseDto> RegisterCandidateAsync(
            RegisterCandidateDto dto)
        {
            if (await _userRepository.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException(
                    "An account with this email already exists.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                Role = UserRole.Candidate,

                CandidateProfile = new CandidateProfile
                {
                    IsDiscoverable = true
                }
            };

            await _userRepository.AddAsync(user);

            // Create verification token and send email
            await IssueVerificationEmailAsync(user);

            // IMPORTANT:
            // No JWT is generated during registration.
            return new RegisterResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Message =
                    "Account created successfully. Please check your email to verify your account."
            };
        }



        public async Task<RegisterResponseDto> RegisterEmployerAsync(
            RegisterEmployerDto dto)
        {
            if (await _userRepository.ExistsByEmailAsync(dto.Email))
                throw new InvalidOperationException(
                    "An account with this email already exists.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                Role = UserRole.Employer,

                EmployerProfile = new EmployerProfile
                {
                    CompanyName = dto.CompanyName
                }
            };

            await _userRepository.AddAsync(user);

            // Create verification token and send email
            await IssueVerificationEmailAsync(user);

            // IMPORTANT:
            // No JWT is generated during registration.
            return new RegisterResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Message =
                    "Account created successfully. Please check your email to verify your account."
            };
        }


        public async Task VerifyEmailAsync(
            VerifyEmailDto dto)
        {
            var token =
                await _verificationTokenRepository
                    .GetValidTokenAsync(dto.Token);

            if (token is null)
            {
                throw new ArgumentException(
                    "This verification link is invalid or has expired. Please request a new one.");
            }

            var user =
                await _userRepository.GetByIdAsync(token.UserId);

            if (user is null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            user.EmailVerifiedAt = DateTime.UtcNow;

            token.IsUsed = true;

            await _userRepository.UpdateAsync(user);

            await _verificationTokenRepository.UpdateAsync(token);
        }


        public async Task<AuthResponseDto> LoginAsync(
            LoginDto dto)
        {
            var user =
                await _userRepository.GetByEmailAsync(dto.Email);

            if (user is null ||
                !_passwordHasher.Verify(
                    dto.Password,
                    user.PasswordHash))
            {
                throw new UnauthorizedAccessException(
                    "Incorrect email or password.");
            }

            if (user.IsSuspended)
            {
                throw new UnauthorizedAccessException(
                    "Your account has been suspended. Please contact support.");
            }

            // IMPORTANT:
            // User cannot receive JWT until email is verified.
            if (user.EmailVerifiedAt is null)
            {
                throw new UnauthorizedAccessException(
                    "Please verify your email before logging in.");
            }

            // JWT is generated ONLY after successful verification.
            return BuildAuthResponse(user);
        }

     
        public async Task ForgotPasswordAsync(
            ForgotPasswordDto dto)
        {
            var user =
                await _userRepository.GetByEmailAsync(dto.Email);

            // Don't reveal whether the email exists.
            if (user is null)
                return;

            var token = new PasswordResetToken
            {
                UserId = user.Id,
                Token = Guid.NewGuid().ToString("N"),
                ExpiresAt = DateTime.UtcNow.AddMinutes(30)
            };

            await _resetTokenRepository.AddAsync(token);

            await _emailService.SendPasswordResetEmailAsync(
                user.Email,
                user.Name,
                token.Token);
        }


        public async Task ResetPasswordAsync(
            ResetPasswordDto dto)
        {
            var token =
                await _resetTokenRepository
                    .GetValidTokenAsync(dto.Token);

            if (token is null)
            {
                throw new ArgumentException(
                    "This password reset link is invalid or has expired. Please request a new one.");
            }

            var user =
                await _userRepository.GetByIdAsync(token.UserId);

            if (user is null)
            {
                throw new InvalidOperationException(
                    "User not found.");
            }

            user.PasswordHash =
                _passwordHasher.Hash(dto.NewPassword);

            token.IsUsed = true;

            await _userRepository.UpdateAsync(user);

            await _resetTokenRepository.UpdateAsync(token);
        }


        public async Task LogoutAsync(
            string jti,
            DateTime tokenExpiresAt)
        {
            if (string.IsNullOrWhiteSpace(jti))
            {
                throw new ArgumentException(
                    "Invalid token.");
            }

            await _revokedTokenRepository.AddAsync(
                jti,
                tokenExpiresAt);
        }

      
       
    }
}