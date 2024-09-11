namespace MiniAssetManagement.UseCases.Folders.List;

public interface IListFoldersQueryService
{
    Task<IEnumerable<FolderDTO>> ListFromDriveAsync(
        int driveId,
        int? skip = null,
        int? take = null
    );
    Task<IEnumerable<FolderDTO>> ListFromFolderAsync(
        int folderId,
        int? skip = null,
        int? take = null
    );
}