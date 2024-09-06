using Ardalis.Specification;

namespace MiniAssetManagement.Core.FolderAggregate.Specifications;

public class FoldersByDriveIdSpec : Specification<Folder>
{
    public FoldersByDriveIdSpec(int driveId) =>
        Query.Where(user => user.DriveId == driveId && user.Status == FolderStatus.Available);
}
