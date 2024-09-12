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
            db.Folders.Where(d => d.DriveId == driveId).Select(d => new FolderDTO(d.Id, d.Name)),
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
            db.Folders.Where(d => d.ParentId == folderId).Select(d => new FolderDTO(d.Id, d.Name)),
            skip,
            take
        );
    }
}
