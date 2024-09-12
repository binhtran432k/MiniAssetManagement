using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.UseCases.Assets.Create;

public class CreateAssetFromAssetHandler(
    IRepository<Asset> _repository,
    IGetAssetPermissionQueryService _permissionQuery
) : ICommandHandler<CreateAssetFromAssetCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateAssetFromAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.AssetId, request.AdminId);
        if (permission != PermissionType.Admin && permission != PermissionType.Contributor)
            return Result.Unauthorized();

        var newAsset = Asset.CreateFromAsset(request.Name, request.AssetId);
        newAsset.AddOrUpdatePermission(request.AdminId, PermissionType.Admin);
        var createdItem = await _repository.AddAsync(newAsset, cancellationToken);

        return createdItem.Id;
    }
}
