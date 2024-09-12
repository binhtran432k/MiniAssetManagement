using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class AssetsByDriveIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidDriveId()
    {
        // Given
        var asset1 = AssetFixture.CreateAssetFromDrive(1, "a", 1);
        var asset2 = AssetFixture.CreateAssetFromDrive(2, "b", 2);
        var asset3 = AssetFixture.CreateAssetFromDrive(3, "c", 1);
        List<Asset> assets = new() { asset1, asset2, asset3 };

        // When
        var spec = new AssetsByDriveIdSpec(1);
        var filteredAssets = spec.Evaluate(assets);

        // Then
        Assert.That(
            filteredAssets,
            Is.EquivalentTo(new List<Asset>() { asset1, asset3 }),
            nameof(filteredAssets)
        );
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithInvalidDriveId()
    {
        // Given
        var asset1 = AssetFixture.CreateAssetFromDrive(1, "a", 1);
        var asset2 = AssetFixture.CreateAssetFromDrive(2, "b", 1);
        var asset3 = AssetFixture.CreateAssetFromDrive(3, "c", 1);
        List<Asset> assets = new() { asset1, asset2, asset3 };

        // When
        var spec = new AssetsByDriveIdSpec(100);
        var filteredAssets = spec.Evaluate(assets);

        // Then
        Assert.That(filteredAssets, Is.Empty, nameof(filteredAssets));
    }
}
