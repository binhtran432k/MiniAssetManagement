using Microsoft.EntityFrameworkCore;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UseCases.Folders.GetPermission;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public class GetFolderPermissionQueryService(AppDbContext db) : IGetFolderPermissionQueryService
{
    public Task<PermissionType?> GetAsync(int folderId, int userId)
    {
        var permission = db
            .Permissions.Where(p => p.FolderId == folderId && p.UserId == userId)
            .FirstOrDefault();

        if (permission is not null)
            return Task.FromResult((PermissionType?)permission.Type);

        Folder? folder = db.Folders.Where(f => f.Id == folderId).FirstOrDefault();
        if (folder is null)
            return Task.FromResult((PermissionType?)null);

        Folder? parent = db
            .Folders.Include(f => f.Permissions)
            .Where(f => f.Id == folder.ParentId)
            .FirstOrDefault();
        while (parent is not null)
        {
            var parentPermission = parent
                .Permissions.Where(p => p.UserId == userId)
                .FirstOrDefault();

            if (parentPermission is not null)
                return Task.FromResult((PermissionType?)parentPermission.Type);

            parent = db
                .Folders.Include(f => f.Permissions)
                .Where(f => f.Id == parent.ParentId)
                .FirstOrDefault();
        }

        return Task.FromResult((PermissionType?)null);
    }
}
