using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiniAssetManagement.Core;

public static class CoreServiceExtensions
{
    public static IServiceCollection AddCoreServices(
        this IServiceCollection services,
        ILogger logger
    )
    {
        logger.LogInformation("{Project} services registered", "Core");

        return services;
    }
}
