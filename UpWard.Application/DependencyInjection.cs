using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Upwork.Application.Interfaces;
using Upwork.Application.Interfaces.IService;
using Upwork.Application.Services;

namespace Upwork.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<IAdminCategoryService, AdminCategoryService>();
            services.AddScoped<IAdminJobService, AdminJobService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAdminTechnologyService, AdminTechnologyService>();
            services.AddScoped<IAdminCommentService, AdminCommentService>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();

            // Auth feature
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IEmployerService, EmployerService>();
            services.AddScoped<IEmployerJobService, EmployerJobService>();
            services.AddScoped<IEmployerApplicationService, EmployerApplicationService>();
            services.AddScoped<IEmployerAnalyticsService, EmployerAnalyticsService>();
            services.AddScoped<ICandidateProfileService, CandidateProfileService>();
            services.AddScoped<ISkillsService, SkillsService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IApplicationService, ApplicationService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}