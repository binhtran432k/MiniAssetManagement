using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.UseCases.Assets.Get;

public class GetAssetHandler(
    IRepository<Asset> _repository,
    IGetAssetPermissionQueryService _permissionQuery
) : IQueryHandler<GetAssetQuery, Result<AssetDTO>>
{
    public async Task<Result<AssetDTO>> Handle(
        GetAssetQuery request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.AssetId, request.UserId);
        if (permission is null)
            return Result.Unauthorized();

        var entity = await _repository.FirstOrDefaultAsync(new AssetByIdSpec(request.AssetId));
        if (entity is null)
            return Result.NotFound();

        return new AssetDTO(entity.Id, entity.Name, entity.FileType);
    }
}
