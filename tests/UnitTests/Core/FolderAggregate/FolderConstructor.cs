using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.FolderAggregate;

public class FolderConstructor
{
    [Test]
    public void InitializesFolderFromDrive()
    {
        // When
        var folder = Folder.CreateFromDrive(FolderFixture.NameDefault, 1);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(folder.Name, Is.EqualTo(FolderFixture.NameDefault), nameof(folder.Name));
            Assert.That(folder.DriveId, Is.EqualTo(1), nameof(folder.DriveId));
            Assert.That(folder.Permissions, Is.Empty, nameof(folder.Permissions));
            Assert.That(folder.Status, Is.EqualTo(FolderStatus.Available), nameof(folder.Status));
        });
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyNameFromDrive()
    {
        Assert.Throws<ArgumentNullException>(
            () => Folder.CreateFromDrive(null!, 1),
            nameof(Folder)
        );
        Assert.Throws<ArgumentException>(() => Folder.CreateFromDrive("", 1), nameof(Folder));
    }

    [Test]
    public void InitializesFolderFromFolder()
    {
        // When
        var folder = Folder.CreateFromFolder(FolderFixture.NameDefault, 1);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(folder.Name, Is.EqualTo(FolderFixture.NameDefault), nameof(folder.Name));
            Assert.That(folder.ParentId, Is.EqualTo(1), nameof(folder.ParentId));
            Assert.That(folder.Permissions, Is.Empty, nameof(folder.Permissions));
            Assert.That(folder.Status, Is.EqualTo(FolderStatus.Available), nameof(folder.Status));
        });
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyNameFromFolder()
    {
        Assert.Throws<ArgumentNullException>(
            () => Folder.CreateFromFolder(null!, 1),
            nameof(Folder)
        );
        Assert.Throws<ArgumentException>(() => Folder.CreateFromFolder("", 1), nameof(Folder));
    }
}