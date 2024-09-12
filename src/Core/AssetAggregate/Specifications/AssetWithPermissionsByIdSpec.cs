using Ardalis.Specification;

namespace MiniAssetManagement.Core.AssetAggregate.Specifications;

public class AssetWithPermissionsByIdSpec : Specification<Asset>
{
    public AssetWithPermissionsByIdSpec(int assetId) =>
        Query
            .Include(asset => asset.Permissions)
            .Where(asset => asset.Id == assetId && asset.Status == AssetStatus.Available);
}
