using Microsoft.EntityFrameworkCore;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public class GetAssetPermissionQueryService(AppDbContext db) : IGetAssetPermissionQueryService
{
    public Task<PermissionType?> GetAsync(int assetId, int userId)
    {
        var permission = db
            .Permissions.Where(p => p.AssetId == assetId && p.UserId == userId)
            .FirstOrDefault();

        if (permission is not null)
            return Task.FromResult((PermissionType?)permission.Type);

        Asset? asset = db.Assets.Where(f => f.Id == assetId).FirstOrDefault();
        if (asset is null)
            return Task.FromResult((PermissionType?)null);

        Asset? parent = db
            .Assets.Include(f => f.Permissions)
            .Where(f => f.Id == asset.ParentId)
            .FirstOrDefault();
        while (parent is not null)
        {
            var parentPermission = parent
                .Permissions.Where(p => p.UserId == userId)
                .FirstOrDefault();

            if (parentPermission is not null)
                return Task.FromResult((PermissionType?)parentPermission.Type);

            parent = db
                .Assets.Include(f => f.Permissions)
                .Where(f => f.Id == parent.ParentId)
                .FirstOrDefault();
        }

        return Task.FromResult((PermissionType?)null);
    }
}
