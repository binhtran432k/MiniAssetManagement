using MiniAssetManagement.Core.AssetAggregate;

namespace MiniAssetManagement.UnitTests.Fixtures;

public static class AssetFixture
{
    public const string NameDefault = "test name";
    public const string NameNew = "new name";
    public const int UserIdContributor = 2;
    public const int DriveIdDefault = 1;
    public const int ParentIdDefault = 2;
    public const int IdDefault = 1;
    public const int IdInvalid = 100;
    public const int IdDeleted = 2;
    public const int IdToDelete = 3;

    public static Asset CreateAssetFromDrive(
        int id,
        string name,
        int driveId,
        AssetStatus? status = null
    )
    {
        var asset = Asset.CreateFromDrive(name, driveId);
        asset.Id = id;
        asset.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);
        if (status is not null)
            asset.UpdateStatus(status);
        return asset;
    }

    public static Asset CreateAssetFromAsset(
        int id,
        string name,
        int assetId,
        AssetStatus? status = null
    )
    {
        var asset = Asset.CreateFromAsset(name, assetId);
        asset.Id = id;
        asset.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);
        if (status is not null)
            asset.UpdateStatus(status);
        return asset;
    }

    public static Asset CreateAssetDefaultFromDrive() =>
        CreateAssetFromDrive(IdDefault, NameDefault, DriveIdDefault);

    public static Asset CreateAssetDefaultFromAsset() =>
        CreateAssetFromAsset(IdDefault, NameDefault, ParentIdDefault);
}
