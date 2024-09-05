using Ardalis.Specification;

namespace MiniAssetManagement.Core.DriveAggregate.Specifications;

public class DrivesByOwnerIdSpec : Specification<Drive>
{
    public DrivesByOwnerIdSpec(int ownerId) =>
        Query.Where(drive => drive.OwnerId == ownerId && drive.Status == DriveStatus.Available);
}