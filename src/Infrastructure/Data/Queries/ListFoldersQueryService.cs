using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UseCases.Folders;
using MiniAssetManagement.UseCases.Folders.List;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public class ListFoldersQueryService(AppDbContext db) : IListFoldersQueryService
{
    public Task<(IEnumerable<FolderDTO>, int)> ListFromDriveAsync(
        int driveId,
        int? skip = null,
        int? take = null
    )
    {
        return ListQuery.ListAsync(
            db.Folders.Where(f => f.DriveId == driveId && f.Status == FolderStatus.Available)
                .Select(f => new FolderDTO(f.Id, f.Name)),
            skip,
            take
        );
    }

    public Task<(IEnumerable<FolderDTO>, int)> ListFromFolderAsync(
        int folderId,
        int? skip = null,
        int? take = null
    )
    {
        return ListQuery.ListAsync(
            db.Folders.Where(f => f.ParentId == folderId && f.Status == FolderStatus.Available)
                .Select(f => new FolderDTO(f.Id, f.Name)),
            skip,
            take
        );
    }
}
