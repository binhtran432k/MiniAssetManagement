using MiniAssetManagement.Core.AssetAggregate;

namespace MiniAssetManagement.UseCases.Assets.GetPermission;

public interface IGetAssetPermissionQueryService
{
    Task<PermissionType?> GetAsync(int assetId, int userId);
}
