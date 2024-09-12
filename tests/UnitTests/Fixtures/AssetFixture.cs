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
    public static readonly FileType FileTypeDefault = FileType.Text;

    public static Asset CreateFolderFromDrive(
        int id,
        string name,
        int driveId,
        AssetStatus? status = null
    ) => PostCreateAsset(id, Asset.CreateFolderFromDrive(name, driveId), status);

    public static Asset CreateFileFromDrive(
        int id,
        string name,
        int driveId,
        FileType type,
        AssetStatus? status = null
    ) => PostCreateAsset(id, Asset.CreateFileFromDrive(name, driveId, type), status);

    public static Asset CreateFolderFromAsset(
        int id,
        string name,
        int assetId,
        AssetStatus? status = null
    ) => PostCreateAsset(id, Asset.CreateFolderFromAsset(name, assetId), status);

    public static Asset CreateFileFromAsset(
        int id,
        string name,
        int assetId,
        FileType type,
        AssetStatus? status = null
    ) => PostCreateAsset(id, Asset.CreateFileFromAsset(name, assetId, type), status);

    public static Asset CreateFolderDefaultFromDrive() =>
        CreateFolderFromDrive(IdDefault, NameDefault, DriveIdDefault);

    public static Asset CreateFileDefaultFromDrive() =>
        CreateFileFromDrive(IdDefault, NameDefault, DriveIdDefault, FileTypeDefault);

    public static Asset CreateFolderDefaultFromAsset() =>
        CreateFolderFromAsset(IdDefault, NameDefault, ParentIdDefault);

    public static Asset CreateFileDefaultFromAsset() =>
        CreateFileFromAsset(IdDefault, NameDefault, ParentIdDefault, FileTypeDefault);

    static Asset PostCreateAsset(int id, Asset asset, AssetStatus? status = null)
    {
        asset.Id = id;
        asset.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);
        if (status is not null)
            asset.UpdateStatus(status);
        return asset;
    }
}
