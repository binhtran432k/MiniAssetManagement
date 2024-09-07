namespace MiniAssetManagement.UseCases.Drives.List;

public interface IListDrivesQueryService
{
    Task<IEnumerable<DriveDTO>> ListAsync(int ownerId, int? skip = null, int? take = null);
}