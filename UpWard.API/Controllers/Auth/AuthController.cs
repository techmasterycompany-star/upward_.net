using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Upwork.Application.DTOs.Auth;
using Upwork.Application.Interfaces.IService;
using static System.Net.Mime.MediaTypeNames;

namespace Upwork.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

       
        [HttpPost("register/candidate")]
        public async Task<IActionResult> RegisterCandidate(
            [FromBody] RegisterCandidateDto request)
        {
            try
            {
                var result =
                    await _authService.RegisterCandidateAsync(request);

                return Ok(new
                {
                    Message = "Account created successfully. Please check your email to verify your account.",
                    Data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred during registration.",
                    Detail = ex.Message
                });
            }
        }


        [HttpPost("register/employer")]
        public async Task<IActionResult> RegisterEmployer(
            [FromBody] RegisterEmployerDto request)
        {
            try
            {
                var result =
                    await _authService.RegisterEmployerAsync(request);

                return Ok(new
                {
                    Message = "Account created successfully. Please check your email to verify your account.",
                    Data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred during registration.",
                    Detail = ex.Message
                });
            }
        }



        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(
            [FromBody] VerifyEmailDto request)
        {
            try
            {
                await _authService.VerifyEmailAsync(request);

                return Ok(new
                {
                    Message = "Your email has been verified successfully. You can now log in."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred during email verification.",
                    Detail = ex.Message
                });
            }
        }

       

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmailFromLink(
            [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Content(
                    """
                    <html>
                        <body>
                            <h2>Verification Failed</h2>
                            <p>Verification token is missing.</p>
                        </body>
                    </html>
                    """,
                    "text/html");
            }

            try
            {
                var request = new VerifyEmailDto
                {
                    Token = token
                };

                await _authService.VerifyEmailAsync(request);

                return Content(
                    """
                    <html>
                        <head>
                            <title>Email Verified</title>
                        </head>
                        <body>
                            <h2>Email Verified Successfully!</h2>
                            <p>Your Upwork account has been verified.</p>
                            <p>You can now log in.</p>
                        </body>
                    </html>
                    """,
                    "text/html");
            }
            catch (ArgumentException ex)
            {
                return Content(
                    $"""
                    <html>
                        <body>
                            <h2>Verification Failed</h2>
                            <p>{System.Net.WebUtility.HtmlEncode(ex.Message)}</p>
                        </body>
                    </html>
                    """,
                    "text/html");
            }
            catch (InvalidOperationException ex)
            {
                return Content(
                    $"""
                    <html>
                        <body>
                            <h2>Verification Failed</h2>
                            <p>{System.Net.WebUtility.HtmlEncode(ex.Message)}</p>
                        </body>
                    </html>
                    """,
                    "text/html");
            }
            catch (Exception)
            {
                return Content(
                    """
                    <html>
                        <body>
                            <h2>Verification Failed</h2>
                            <p>An unexpected error occurred while verifying your email.</p>
                        </body>
                    </html>
                    """,
                    "text/html");
            }
        }

      

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto request)
        {
            try
            {
                var result =
                    await _authService.LoginAsync(request);

                return Ok(new
                {
                    Message = "Logged in successfully.",
                    Data = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred during login.",
                    Detail = ex.Message
                });
            }
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordDto request)
        {
            try
            {
                await _authService.ForgotPasswordAsync(request);

                return Ok(new
                {
                    Message = "If an account with that email exists, a password reset link has been sent."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred during forgot password request.",
                    Detail = ex.Message
                });
            }
        }

      
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordDto request)
        {
            try
            {
                await _authService.ResetPasswordAsync(request);

                return Ok(new
                {
                    Message = "Your password has been reset successfully. You can now log in with your new password."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred during password reset.",
                    Detail = ex.Message
                });
            }
        }

      
      
      
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var jti =
                    User.FindFirstValue(JwtRegisteredClaimNames.Jti);

                var expClaim =
                    User.FindFirstValue(JwtRegisteredClaimNames.Exp);

                if (string.IsNullOrWhiteSpace(jti) ||
                    string.IsNullOrWhiteSpace(expClaim))
                {
                    return BadRequest(new
                    {
                        Message = "Invalid token."
                    });
                }

                var expiresAt =
                    DateTimeOffset
                        .FromUnixTimeSeconds(long.Parse(expClaim))
                        .UtcDateTime;

                await _authService.LogoutAsync(
                    jti,
                    expiresAt);

                return Ok(new
                {
                    Message = "Logged out successfully."
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred during logout.",
                    Detail = ex.Message
                });
            }
        }
    }
}