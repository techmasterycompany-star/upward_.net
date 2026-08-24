using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Upward.Application.Interfaces.IRepo;
using Upward.Infrastructure.Data;
using Upward.Infrastructure.Repositories;

namespace Upward.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IEmployerRepository, EmployerRepository>();
            services.AddScoped<IEmployerJobRepository, EmployerJobRepository>();
            services.AddScoped<IEmployerApplicationRepository, EmployerApplicationRepository>();
            services.AddScoped<IEmployerAnalyticsRepository, EmployerAnalyticsRepository>();

            return services;
        }
    }
}
