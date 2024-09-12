using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.UseCases.Assets.Update;

public class UpdateAssetHandler(
    IRepository<Asset> _repository,
    IGetAssetPermissionQueryService _permissionQuery
) : ICommandHandler<UpdateAssetCommand, Result<AssetDTO>>
{
    public async Task<Result<AssetDTO>> Handle(
        UpdateAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.AssetId, request.UserId);
        if (permission != PermissionType.Admin && permission != PermissionType.Contributor)
            return Result.Unauthorized();

        var existingAsset = await _repository.FirstOrDefaultAsync(
            new AssetByIdSpec(request.AssetId)
        );
        if (existingAsset is null)
            return Result.NotFound();

        existingAsset.UpdateName(request.NewName);
        await _repository.UpdateAsync(existingAsset, cancellationToken);

        return new AssetDTO(existingAsset.Id, existingAsset.Name, existingAsset.FileType);
    }
}
