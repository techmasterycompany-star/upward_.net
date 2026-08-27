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
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
