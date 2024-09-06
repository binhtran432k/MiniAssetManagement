using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.FolderAggregate;

public class Folder_AddPermission
{
    [Test]
    public void AddsPermission()
    {
        // Given
        Folder folder = Folder.CreateFromDrive(FolderFixture.NameDefault, 1);

        // When
        folder.AddPermission(new(1, PermissionType.Admin));

        // Then
        Assert.That(
            folder.Permissions,
            Is.EquivalentTo(new List<Permission>() { new(1, PermissionType.Admin) }),
            nameof(folder.Permissions)
        );
    }
}
