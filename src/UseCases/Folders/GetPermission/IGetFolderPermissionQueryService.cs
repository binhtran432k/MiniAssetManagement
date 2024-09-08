using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.UseCases.Folders.GetPermission;

public interface IGetFolderPermissionQueryService
{
    Task<PermissionType?> GetAsync(int folderId, int userId);
}