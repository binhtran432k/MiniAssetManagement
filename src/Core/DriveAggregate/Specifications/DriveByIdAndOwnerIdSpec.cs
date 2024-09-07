using Ardalis.Specification;

namespace MiniAssetManagement.Core.DriveAggregate.Specifications;

public class DriveByIdAndOwnerIdSpec : Specification<Drive>
{
    public DriveByIdAndOwnerIdSpec(int id, int ownerId) =>
        Query.Where(drive =>
            drive.Id == id && drive.OwnerId == ownerId && drive.Status == DriveStatus.Available
        );
}
