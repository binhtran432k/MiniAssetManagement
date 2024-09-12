using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.AssetAggregate;

public class AssetConstructor
{
    [Test]
    public void InitializesFolderFromDriveSuccess()
    {
        // When
        var asset = Asset.CreateFolderFromDrive(AssetFixture.NameDefault, 1);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(asset.Name, Is.EqualTo(AssetFixture.NameDefault), nameof(asset.Name));
            Assert.That(asset.DriveId, Is.EqualTo(1), nameof(asset.DriveId));
            Assert.That(asset.Permissions, Is.Empty, nameof(asset.Permissions));
            Assert.That(asset.Status, Is.EqualTo(AssetStatus.Available), nameof(asset.Status));
            Assert.That(asset.FileType, Is.Null, nameof(asset.FileType));
        });
    }

    [TestCase(nameof(FileType.Text))]
    [TestCase(nameof(FileType.Document))]
    [TestCase(nameof(FileType.PDF))]
    [TestCase(nameof(FileType.Unknown))]
    public void InitializesFileFromDriveSuccess(string typeName)
    {
        // When
        var type = FileType.FromName(typeName);
        var asset = Asset.CreateFileFromDrive(AssetFixture.NameDefault, 1, type);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(asset.Name, Is.EqualTo(AssetFixture.NameDefault), nameof(asset.Name));
            Assert.That(asset.DriveId, Is.EqualTo(1), nameof(asset.DriveId));
            Assert.That(asset.Permissions, Is.Empty, nameof(asset.Permissions));
            Assert.That(asset.Status, Is.EqualTo(AssetStatus.Available), nameof(asset.Status));
            Assert.That(asset.FileType, Is.EqualTo(type), nameof(asset.FileType));
        });
    }

    [Test]
    public void InitializesFolderFromAssetSuccess()
    {
        // When
        var asset = Asset.CreateFolderFromAsset(AssetFixture.NameDefault, 1);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(asset.Name, Is.EqualTo(AssetFixture.NameDefault), nameof(asset.Name));
            Assert.That(asset.ParentId, Is.EqualTo(1), nameof(asset.ParentId));
            Assert.That(asset.Permissions, Is.Empty, nameof(asset.Permissions));
            Assert.That(asset.Status, Is.EqualTo(AssetStatus.Available), nameof(asset.Status));
            Assert.That(asset.FileType, Is.Null, nameof(asset.FileType));
        });
    }

    [TestCase(nameof(FileType.Text))]
    [TestCase(nameof(FileType.Document))]
    [TestCase(nameof(FileType.PDF))]
    [TestCase(nameof(FileType.Unknown))]
    public void InitializesFileFromAssetSuccess(string typeName)
    {
        // When
        var type = FileType.FromName(typeName);
        var asset = Asset.CreateFileFromAsset(AssetFixture.NameDefault, 1, type);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(asset.Name, Is.EqualTo(AssetFixture.NameDefault), nameof(asset.Name));
            Assert.That(asset.ParentId, Is.EqualTo(1), nameof(asset.ParentId));
            Assert.That(asset.Permissions, Is.Empty, nameof(asset.Permissions));
            Assert.That(asset.Status, Is.EqualTo(AssetStatus.Available), nameof(asset.Status));
            Assert.That(asset.FileType, Is.EqualTo(type), nameof(asset.FileType));
        });
    }

    [TestCase(null, typeof(ArgumentNullException))]
    [TestCase("", typeof(ArgumentException))]
    public void InitializesFolderFromDriveFailByInvalidInput(string? name, Type expectedType)
    {
        Assert.Throws(expectedType, () => Asset.CreateFolderFromDrive(name!, 1), nameof(Asset));
    }

    [TestCase(null, typeof(ArgumentNullException))]
    [TestCase("", typeof(ArgumentException))]
    public void InitializesFileFromDriveFailByInvalidInput(string? name, Type expectedType)
    {
        Assert.Throws(
            expectedType,
            () => Asset.CreateFileFromDrive(name!, 1, FileType.Text),
            nameof(Asset)
        );
    }

    [TestCase(null, typeof(ArgumentNullException))]
    [TestCase("", typeof(ArgumentException))]
    public void InitializesFolderFromAssetFailByInvalidInput(string? name, Type expectedType)
    {
        Assert.Throws(expectedType, () => Asset.CreateFolderFromAsset(name!, 1), nameof(Asset));
    }

    [TestCase(null, typeof(ArgumentNullException))]
    [TestCase("", typeof(ArgumentException))]
    public void InitializesFileFromAssetFailByInvalidInput(string? name, Type expectedType)
    {
        Assert.Throws(
            expectedType,
            () => Asset.CreateFileFromAsset(name!, 1, FileType.Text),
            nameof(Asset)
        );
    }
}
