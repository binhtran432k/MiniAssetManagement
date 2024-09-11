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
        folder.Id = FolderFixture.IdDefault;

        // When
        folder.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);

        // Then
        Assert.That(
            folder.Permissions,
            Is.EquivalentTo(
                new List<Permission>()
                {
                    new(FolderFixture.IdDefault, UserFixture.IdDefault, PermissionType.Admin),
                }
            ),
            nameof(folder.Permissions)
        );
    }

    [Test]
    public void UpdatesPermissionSuccess()
    {
        // Given
        var folder = FolderFixture.CreateFolderDefaultFromDrive();
        var testPermission = PermissionType.Contributor;

        // When
        folder.AddOrUpdatePermission(UserFixture.IdDefault, testPermission);

        // Then
        Assert.That(
            folder.Permissions,
            Is.EquivalentTo(
                new List<Permission>()
                {
                    new(FolderFixture.IdDefault, UserFixture.IdDefault, testPermission),
                }
            ),
            nameof(folder.Permissions)
        );
    }
}
