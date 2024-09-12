using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.AssetAggregate;

public class AssetConstructor
{
    [Test]
    public void InitializesAssetFromDrive()
    {
        // When
        var asset = Asset.CreateFromDrive(AssetFixture.NameDefault, 1);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(asset.Name, Is.EqualTo(AssetFixture.NameDefault), nameof(asset.Name));
            Assert.That(asset.DriveId, Is.EqualTo(1), nameof(asset.DriveId));
            Assert.That(asset.Permissions, Is.Empty, nameof(asset.Permissions));
            Assert.That(asset.Status, Is.EqualTo(AssetStatus.Available), nameof(asset.Status));
        });
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyNameFromDrive()
    {
        Assert.Throws<ArgumentNullException>(
            () => Asset.CreateFromDrive(null!, 1),
            nameof(Asset)
        );
        Assert.Throws<ArgumentException>(() => Asset.CreateFromDrive("", 1), nameof(Asset));
    }

    [Test]
    public void InitializesAssetFromAsset()
    {
        // When
        var asset = Asset.CreateFromAsset(AssetFixture.NameDefault, 1);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(asset.Name, Is.EqualTo(AssetFixture.NameDefault), nameof(asset.Name));
            Assert.That(asset.ParentId, Is.EqualTo(1), nameof(asset.ParentId));
            Assert.That(asset.Permissions, Is.Empty, nameof(asset.Permissions));
            Assert.That(asset.Status, Is.EqualTo(AssetStatus.Available), nameof(asset.Status));
        });
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyNameFromAsset()
    {
        Assert.Throws<ArgumentNullException>(
            () => Asset.CreateFromAsset(null!, 1),
            nameof(Asset)
        );
        Assert.Throws<ArgumentException>(() => Asset.CreateFromAsset("", 1), nameof(Asset));
    }
}
