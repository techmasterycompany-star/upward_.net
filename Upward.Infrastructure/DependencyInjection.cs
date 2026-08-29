using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Upwork.Application.Interfaces.IRepo;
using Upwork.Application.Interfaces;
using Upwork.Infrastructure.Repositories;
using Upwork.Infrastructure.Stripe;
using Upwork.Application.Interfaces.IService;
using Upwork.Infrastructure.Data;
using Upwork.Infrastructure.FileStorage;

namespace Upwork.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.Configure<CloudinarySettings>(options =>
            {
                options.CloudName = configuration["CLOUDINARY_CLOUD_NAME"] ?? string.Empty;
                options.ApiKey = configuration["CLOUDINARY_API_KEY"] ?? string.Empty;
                options.ApiSecret = configuration["CLOUDINARY_API_SECRET"] ?? string.Empty;
            });
            
            services.AddScoped<INotificationRepository, NotificationRepository>();

            // Repositories
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IEmployerRepository, EmployerRepository>();

            // Stripe
            services.AddScoped<IStripeService, StripeService>();

            services.AddScoped<ICandidateProfileRepository, CandidateProfileRepository>();
            services.AddScoped<ISkillsRepository, SkillsRepository>();
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITechnologyRepository, TechnologyRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IAdminDashboardRepository, AdminDashboardRepository>();
            services.AddScoped<IEmployerJobRepository, EmployerJobRepository>();
            services.AddScoped<IEmployerApplicationRepository, EmployerApplicationRepository>();
            services.AddScoped<IEmployerAnalyticsRepository, EmployerAnalyticsRepository>();

            // Auth feature
            services.AddScoped<IUserAuthRepository, UserAuthRepository>();
            services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
            services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
            services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();

  

            services.AddScoped<IStorageService, CloudinaryStorageService>();
            return services;
        }
    }
}