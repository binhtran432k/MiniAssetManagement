using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.UseCases.Assets.Create;

public class CreateFolderFromAssetHandler(
    IRepository<Asset> repository,
    IGetAssetPermissionQueryService permissionQuery
) : ICommandHandler<CreateFolderFromAssetCommand, Result<int>>
{
    public Task<Result<int>> Handle(
        CreateFolderFromAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        return CreateAssetUtils.CreateFromAssetAsync(
            repository,
            permissionQuery,
            () => Asset.CreateFolderFromAsset(request.Name, request.AssetId),
            assetId: request.AssetId,
            adminId: request.AdminId,
            cancellationToken
        );
    }
}

public class CreateFileFromAssetHandler(
    IRepository<Asset> repository,
    IGetAssetPermissionQueryService permissionQuery
) : ICommandHandler<CreateFileFromAssetCommand, Result<int>>
{
    public Task<Result<int>> Handle(
        CreateFileFromAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        return CreateAssetUtils.CreateFromAssetAsync(
            repository,
            permissionQuery,
            () => Asset.CreateFileFromAsset(request.Name, request.AssetId, request.Type),
            assetId: request.AssetId,
            adminId: request.AdminId,
            cancellationToken
        );
    }
}

public record CreateFolderFromAssetCommand(string Name, int AdminId, int AssetId)
    : ICommand<Result<int>>;

public record CreateFileFromAssetCommand(string Name, int AdminId, int AssetId, FileType Type)
    : ICommand<Result<int>>;
