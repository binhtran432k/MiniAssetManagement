using Ardalis.Specification;

namespace MiniAssetManagement.Core.DriveAggregate.Specifications;

public class DriveByIdSpec : Specification<Drive>
{
    public DriveByIdSpec(int id) =>
        Query.Where(drive => drive.Id == id && drive.Status == DriveStatus.Available);
}
