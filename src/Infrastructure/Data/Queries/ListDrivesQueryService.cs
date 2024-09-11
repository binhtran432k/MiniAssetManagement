using MiniAssetManagement.UseCases.Drives;
using MiniAssetManagement.UseCases.Drives.List;

namespace MiniAssetManagement.Infrastructure.Data.Queries;

public class ListDrivesQueryService(AppDbContext db) : IListDrivesQueryService
{
    public Task<IEnumerable<DriveDTO>> ListAsync(int ownerId, int? skip = null, int? take = null)
    {
        return ListQuery.ListAsync(
            db,
            db.Drives.Where(d => d.OwnerId == ownerId).Select(d => new DriveDTO(d.Id, d.Name)),
            skip,
            take
        );
    }
}