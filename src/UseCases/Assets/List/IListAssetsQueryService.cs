namespace MiniAssetManagement.UseCases.Assets.List;

public interface IListAssetsQueryService
{
    Task<(IEnumerable<AssetDTO>, int)> ListFromDriveAsync(
        int driveId,
        int? skip = null,
        int? take = null
    );
    Task<(IEnumerable<AssetDTO>, int)> ListFromAssetAsync(
        int assetId,
        int? skip = null,
        int? take = null
    );
}
