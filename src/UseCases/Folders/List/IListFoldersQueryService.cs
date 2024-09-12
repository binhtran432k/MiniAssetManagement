namespace MiniAssetManagement.UseCases.Folders.List;

public interface IListFoldersQueryService
{
    Task<(IEnumerable<FolderDTO>, int)> ListFromDriveAsync(
        int driveId,
        int? skip = null,
        int? take = null
    );
    Task<(IEnumerable<FolderDTO>, int)> ListFromFolderAsync(
        int folderId,
        int? skip = null,
        int? take = null
    );
}