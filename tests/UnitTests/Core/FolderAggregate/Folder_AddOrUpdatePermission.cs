using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.FolderAggregate;

public class Folder_AddOrUpdatePermission
{
    [Test]
    public void AddsPermissionSuccess()
    {
        // Given
        Folder folder = Folder.CreateFromDrive(
            FolderFixture.NameDefault,
            FolderFixture.DriveIdDefault
        );

        // When
        folder.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);

        // Then
        Assert.That(
            folder.Permissions,
            Is.EquivalentTo(
                new List<Permission>() { new(UserFixture.IdDefault, PermissionType.Admin) }
            ),
            nameof(folder.Permissions)
        );
    }

    [Test]
    public void UpdatesPermissionSuccess()
    {
        // Given
        Folder folder = Folder.CreateFromDrive(
            FolderFixture.NameDefault,
            FolderFixture.DriveIdDefault
        );
        folder.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);

        // When
        folder.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Contributor);

        // Then
        Assert.That(
            folder.Permissions,
            Is.EquivalentTo(
                new List<Permission>() { new(UserFixture.IdDefault, PermissionType.Contributor) }
            ),
            nameof(folder.Permissions)
        );
    }
}
