using Ardalis.Specification;

namespace MiniAssetManagement.Core.AssetAggregate.Specifications;

public class AssetByIdSpec : Specification<Asset>
{
    public AssetByIdSpec(int assetId) =>
        Query.Where(asset => asset.Id == assetId && asset.Status == AssetStatus.Available);
}
