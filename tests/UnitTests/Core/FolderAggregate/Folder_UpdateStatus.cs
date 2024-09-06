using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.FolderAggregate;

public class Folder_UpdateStatus
{
    [Test]
    public void UpdatesStatus()
    {
        // Given
        Folder folder = Folder.CreateFromDrive(FolderFixture.NameDefault, 1);

        // When
        folder.UpdateStatus(FolderStatus.Deleted);

        // Then
        Assert.That(folder.Status, Is.EqualTo(FolderStatus.Deleted), nameof(folder.Status));
    }
}
