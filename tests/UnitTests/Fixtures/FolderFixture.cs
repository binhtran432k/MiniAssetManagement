using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.UnitTests.Fixtures;

public static class FolderFixture
{
    public const string NameDefault = "test name";
    public const string NameNew = "new name";
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
        if (status != null)
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
        if (status != null)
            folder.UpdateStatus(status);
        return folder;
    }
}
