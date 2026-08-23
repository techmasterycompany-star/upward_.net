using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Upward.Application.Interfaces.IRepo;
using Upward.Application.Interfaces.IService;
using Upward.Infrastructure.Data;
using Upward.Infrastructure.FileStorage;
using Upward.Infrastructure.Repositories;

namespace Upward.Infrastructure
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

            services.AddScoped<ICandidateProfileRepository, CandidateProfileRepository>();
            services.AddScoped<ISkillsRepository, SkillsRepository>();
            services.AddScoped<IStorageService, CloudinaryStorageService>();
            return services;
        }
    }
}
