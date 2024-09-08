using Ardalis.Specification;

namespace MiniAssetManagement.Core.FolderAggregate.Specifications;

public class FolderWithPermissionsByIdSpec : Specification<Folder>
{
    public FolderWithPermissionsByIdSpec(int folderId) =>
        Query
            .Include(folder => folder.Permissions)
            .Where(folder => folder.Id == folderId && folder.Status == FolderStatus.Available);
}