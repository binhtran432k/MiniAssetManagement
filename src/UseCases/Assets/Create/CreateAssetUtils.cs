using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.UseCases.Assets.Create;

static class CreateAssetUtils
{
    public static async Task<Result<int>> CreateFromAssetAsync(
        IRepository<Asset> repository,
        IGetAssetPermissionQueryService permissionQuery,
        Func<Asset> createAssetFunc,
        int assetId,
        int adminId,
        CancellationToken cancellationToken
    )
    {
        var permission = await permissionQuery.GetAsync(assetId, adminId);
        if (permission != PermissionType.Admin && permission != PermissionType.Contributor)
            return Result.Unauthorized();

        var parentAsset = await repository.FirstOrDefaultAsync(
            new AssetByIdSpec(assetId),
            cancellationToken
        );
        if (parentAsset?.FileType is not null)
            return Result.Conflict("Asset must be folder");

        return await CreateAsync(repository, createAssetFunc, adminId, cancellationToken);
    }

    public static async Task<Result<int>> CreateFromDriveAsync(
        IRepository<Asset> repository,
        IRepository<Drive> driveRepository,
        Func<Asset> createAssetFunc,
        int driveId,
        int adminId,
        CancellationToken cancellationToken
    )
    {
        bool hasPermission =
            await driveRepository.CountAsync(new DriveByIdAndOwnerIdSpec(driveId, adminId)) > 0;
        if (!hasPermission)
            return Result.Unauthorized();

        return await CreateAsync(repository, createAssetFunc, adminId, cancellationToken);
    }

    public static async Task<Result<int>> CreateAsync(
        IRepository<Asset> repository,
        Func<Asset> createAssetFunc,
        int adminId,
        CancellationToken cancellationToken
    )
    {
        var newAsset = createAssetFunc();
        newAsset.AddOrUpdatePermission(adminId, PermissionType.Admin);
        var createdItem = await repository.AddAsync(newAsset, cancellationToken);

        return createdItem.Id;
    }
}
