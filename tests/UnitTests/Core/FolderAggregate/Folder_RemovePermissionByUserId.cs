using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.FolderAggregate;

public class Folder_RemovePermissionByUserId
{
    [Test]
    public void RemovesPermissionSuccess()
    {
        // Given
        Folder folder = Folder.CreateFromDrive(
            FolderFixture.NameDefault,
            FolderFixture.DriveIdDefault
        );
        folder.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);

        // When
        folder.RemovePermissionByUserId(UserFixture.IdDefault);

        // Then
        Assert.That(folder.Permissions, Is.Empty, nameof(folder.Permissions));
    }
}
