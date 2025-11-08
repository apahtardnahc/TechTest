using UserManagement.Repository.Implementations;
using UserManagement.Repository.Interfaces;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ILogRepository, LogRepository>();
        return services;
    }
}
