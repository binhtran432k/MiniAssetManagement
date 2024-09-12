using Ardalis.Specification;

namespace MiniAssetManagement.Core.AssetAggregate.Specifications;

public class AssetsByParentIdSpec : Specification<Asset>
{
    public AssetsByParentIdSpec(int parentId) =>
        Query.Where(user => user.ParentId == parentId && user.Status == AssetStatus.Available);
}
