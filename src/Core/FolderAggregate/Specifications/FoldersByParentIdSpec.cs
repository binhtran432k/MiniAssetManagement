using Ardalis.Specification;

namespace MiniAssetManagement.Core.FolderAggregate.Specifications;

public class FoldersByParentIdSpec : Specification<Folder>
{
    public FoldersByParentIdSpec(int parentId) =>
        Query.Where(user => user.ParentId == parentId && user.Status == FolderStatus.Available);
}
