using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.UnitTests.Fixtures;

public static class FolderFixture
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

    public static Folder CreateFolderFromDrive(
        int id,
        string name,
        int driveId,
        FolderStatus? status = null
    )
    {
        var folder = Folder.CreateFromDrive(name, driveId);
        folder.Id = id;
        folder.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);
        if (status is not null)
            folder.UpdateStatus(status);
        return folder;
    }

    public static Folder CreateFolderFromFolder(
        int id,
        string name,
        int folderId,
        FolderStatus? status = null
    )
    {
        var folder = Folder.CreateFromFolder(name, folderId);
        folder.Id = id;
        folder.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);
        if (status is not null)
            folder.UpdateStatus(status);
        return folder;
    }

    public static Folder CreateFolderDefaultFromDrive() =>
        CreateFolderFromDrive(IdDefault, NameDefault, DriveIdDefault);

    public static Folder CreateFolderDefaultFromFolder() =>
        CreateFolderFromFolder(IdDefault, NameDefault, ParentIdDefault);
}
