using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.FolderAggregate;

public class Folder_UpdateName
{
    [Test]
    public void UpdatesName()
    {
        // Given
        var folder = Folder.CreateFromDrive(FolderFixture.NameDefault, 1);

        // When
        folder.UpdateName(FolderFixture.NameNew);

        // Then
        Assert.That(folder.Name, Is.EqualTo(FolderFixture.NameNew), nameof(folder.Name));
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyName()
    {
        var folder = Folder.CreateFromDrive(FolderFixture.NameDefault, 1);

        Assert.Throws<ArgumentNullException>(() => folder.UpdateName(null!), nameof(folder));
        Assert.Throws<ArgumentException>(() => folder.UpdateName(""), nameof(folder));
    }
}