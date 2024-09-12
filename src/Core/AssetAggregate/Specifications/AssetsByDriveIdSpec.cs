using Ardalis.Specification;

namespace MiniAssetManagement.Core.AssetAggregate.Specifications;

public class AssetsByDriveIdSpec : Specification<Asset>
{
    public AssetsByDriveIdSpec(int driveId) =>
        Query.Where(user => user.DriveId == driveId && user.Status == AssetStatus.Available);
}
