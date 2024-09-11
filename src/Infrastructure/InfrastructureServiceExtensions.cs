using Ardalis.SharedKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Infrastructure.Data;
using MiniAssetManagement.Infrastructure.Data.Queries;
using MiniAssetManagement.UseCases.Drives.List;
using MiniAssetManagement.UseCases.Folders.GetPermission;
using MiniAssetManagement.UseCases.Folders.List;
using MiniAssetManagement.UseCases.Users.List;

namespace MiniAssetManagement.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        ILogger logger
    )
    {
        RegisterDependencies(services);

        RegisterEF(services);

        logger.LogInformation("{Project} services registered", "Infrastructure");

        return services;
    }

    private static void RegisterDependencies(IServiceCollection services)
    {
        services.AddScoped<IListUsersQueryService, ListUsersQueryService>();
        services.AddScoped<IListDrivesQueryService, ListDrivesQueryService>();
        services.AddScoped<IListFoldersQueryService, ListFoldersQueryService>();
        services.AddScoped<IGetFolderPermissionQueryService, GetFolderPermissionQueryService>();
    }

    private static void RegisterEF(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));
    }
}