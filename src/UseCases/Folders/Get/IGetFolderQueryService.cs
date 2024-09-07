using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.UseCases.Folders.Get;

public interface IGetFolderQueryService
{
    Task<Folder?> GetAsync(int folderId, int userId);
}