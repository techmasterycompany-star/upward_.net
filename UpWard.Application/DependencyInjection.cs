using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Upward.Application.Interfaces.IService;
using Upward.Application.Services;


namespace Upward.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddAutoMapper(cfg => cfg.AddMaps(typeof(DependencyInjection).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<IAdminCategoryService, AdminCategoryService>();
            services.AddScoped<IAdminJobService, AdminJobService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IAdminTechnologyService, AdminTechnologyService>();
            services.AddScoped<IAdminCommentService, AdminCommentService>();
            services.AddScoped<IAdminDashboardService, AdminDashboardService>();

            return services;
        }
    }
}
