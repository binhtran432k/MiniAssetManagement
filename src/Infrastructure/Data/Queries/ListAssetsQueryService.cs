using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UseCases.Assets;
using MiniAssetManagement.UseCases.Assets.List;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public class ListAssetsQueryService(AppDbContext db) : IListAssetsQueryService
{
    public Task<(IEnumerable<AssetDTO>, int)> ListFromDriveAsync(
        int driveId,
        int? skip = null,
        int? take = null
    )
    {
        return ListQuery.ListAsync(
            db.Assets.Where(f => f.DriveId == driveId && f.Status == AssetStatus.Available)
                .Select(f => new AssetDTO(f.Id, f.Name)),
            skip,
            take
        );
    }

    public Task<(IEnumerable<AssetDTO>, int)> ListFromAssetAsync(
        int assetId,
        int? skip = null,
        int? take = null
    )
    {
        return ListQuery.ListAsync(
            db.Assets.Where(f => f.ParentId == assetId && f.Status == AssetStatus.Available)
                .Select(f => new AssetDTO(f.Id, f.Name)),
            skip,
            take
        );
    }
}
