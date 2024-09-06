using Ardalis.Specification;

namespace MiniAssetManagement.Core.FolderAggregate.Specifications;

public class FolderByIdSpec : Specification<Folder>
{
    public FolderByIdSpec(int folderId) =>
        Query.Where(folder => folder.Id == folderId && folder.Status == FolderStatus.Available);
}
