using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Upwork.Application.Interfaces.IService;

namespace Upwork.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendVerificationEmailAsync(
     string toEmail,
     string userName,
     string token)
        {
            var verificationUrl =
                _configuration["EmailSettings:VerificationUrl"];

            var verificationLink =
                $"{verificationUrl}?token={Uri.EscapeDataString(token)}";

            var subject = "Verify Your Upwork Account";

            var body = $"""
        Hello {userName},

        Thank you for registering for Upwork.

        Please click the link below to verify your email address:

        {verificationLink}

        This verification link will expire after 24 hours.

        If you did not create this account, please ignore this email.

        Regards,
        Upwork Team
        """;

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(
     string toEmail,
     string userName,
     string token)
        {
            var resetUrl =
                _configuration["EmailSettings:ResetPasswordUrl"];

            var resetLink =
                $"{resetUrl}?token={Uri.EscapeDataString(token)}";

            var subject = "Reset Your Upwork Password";

            var body = $"""
        Hello {userName},

        We received a request to reset your Upwork password.

        Please use the following link to get your password reset token:

        {resetLink}

        This reset token will expire after 30 minutes.

        If you did not request a password reset, please ignore this email.

        Regards,
        Upwork Team
        """;

            await SendEmailAsync(toEmail, subject, body);
        }

        private async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var smtpServer =
                _configuration["EmailSettings:SmtpServer"];

            var smtpPort =
                int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");

            var senderEmail =
                _configuration["EmailSettings:SenderEmail"];

            var senderName =
                _configuration["EmailSettings:SenderName"];

            var username =
                _configuration["EmailSettings:Username"];

            var password =
                _configuration["EmailSettings:Password"];

            var enableSsl =
                bool.Parse(
                    _configuration["EmailSettings:EnableSsl"] ?? "true");

            using var message = new MailMessage();

            message.From = new MailAddress(
                senderEmail!,
                senderName);

            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = false;

            using var smtpClient = new SmtpClient(
                smtpServer,
                smtpPort);

            smtpClient.EnableSsl = enableSsl;

            smtpClient.Credentials = new NetworkCredential(
                username,
                password);

            await smtpClient.SendMailAsync(message);
        }
    }
}