using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.UseCases.Assets.RemovePermission;

public class RemoveAssetPermissionHandler(
    IRepository<Asset> _repository,
    IGetAssetPermissionQueryService _permissionQuery
) : ICommandHandler<RemoveAssetPermissionCommand, Result>
{
    public async Task<Result> Handle(
        RemoveAssetPermissionCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.UserId == request.RemoveUserId)
            return Result.Conflict();

        var permission = await _permissionQuery.GetAsync(request.AssetId, request.UserId);
        if (permission != PermissionType.Admin)
            return Result.Unauthorized();

        var existingAsset = await _repository.FirstOrDefaultAsync(
            new AssetWithPermissionsByIdSpec(request.AssetId),
            cancellationToken
        );
        if (existingAsset is null)
            return Result.NotFound();

        existingAsset.RemovePermissionByUserId(request.RemoveUserId);

        await _repository.UpdateAsync(existingAsset);

        return Result.Success();
    }
}