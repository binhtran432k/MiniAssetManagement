using MiniAssetManagement.Core.DriveAggregate;

namespace MiniAssetManagement.UnitTests.Fixtures;

public static class DriveFixture
{
    public const string NameDefault = "test name";
    public const string NameNew = "new name";
    public const int OwnerIdDefault = 1;
    public const int IdDefault = 1;
    public const int IdInvalid = 100;
    public const int IdDeleted = 2;
    public const int IdToDelete = 3;

    public static Drive CreateDrive(int id, string name, int ownerId, DriveStatus? status = null)
    {
        Drive drive = new(name, ownerId);
        drive.Id = id;
        if (status != null)
            drive.UpdateStatus(status);
        return drive;
    }

    public static Drive CreateDriveDefault() => CreateDrive(IdDefault, NameDefault, OwnerIdDefault);
}