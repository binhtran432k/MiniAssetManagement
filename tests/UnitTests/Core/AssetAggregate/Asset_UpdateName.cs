using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.AssetAggregate;

public class Asset_UpdateName
{
    [Test]
    public void UpdatesName()
    {
        // Given
        var asset = Asset.CreateFolderFromDrive(AssetFixture.NameDefault, 1);

        // When
        asset.UpdateName(AssetFixture.NameNew);

        // Then
        Assert.That(asset.Name, Is.EqualTo(AssetFixture.NameNew), nameof(asset.Name));
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyName()
    {
        var asset = Asset.CreateFolderFromDrive(AssetFixture.NameDefault, 1);

        Assert.Throws<ArgumentNullException>(() => asset.UpdateName(null!), nameof(asset));
        Assert.Throws<ArgumentException>(() => asset.UpdateName(""), nameof(asset));
    }
}
