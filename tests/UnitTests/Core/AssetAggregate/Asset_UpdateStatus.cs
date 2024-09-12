using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.AssetAggregate;

public class Asset_UpdateStatus
{
    [Test]
    public void UpdatesStatus()
    {
        // Given
        Asset asset = Asset.CreateFolderFromDrive(AssetFixture.NameDefault, 1);

        // When
        asset.UpdateStatus(AssetStatus.Deleted);

        // Then
        Assert.That(asset.Status, Is.EqualTo(AssetStatus.Deleted), nameof(asset.Status));
    }
}
