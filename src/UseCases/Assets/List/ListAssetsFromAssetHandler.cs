using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.UseCases.Assets.List;

public class ListAssetsFromAssetHandler(
    IListAssetsQueryService _query,
    IGetAssetPermissionQueryService _permissionQuery
) : IQueryHandler<ListAssetsFromAssetQuery, Result<(IEnumerable<AssetDTO>, int)>>
{
    public async Task<Result<(IEnumerable<AssetDTO>, int)>> Handle(
        ListAssetsFromAssetQuery request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.AssetId, request.UserId);
        if (permission is null)
            return Result.Unauthorized();

        var result = await _query.ListFromAssetAsync(request.AssetId, request.Skip, request.Take);
        return Result.Success(result);
    }
}
