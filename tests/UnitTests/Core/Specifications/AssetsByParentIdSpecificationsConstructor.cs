using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class AssetsByParentIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidParentId()
    {
        // Given
        var asset1 = AssetFixture.CreateAssetFromAsset(1, "a", 1);
        var asset2 = AssetFixture.CreateAssetFromAsset(2, "b", 2);
        var asset3 = AssetFixture.CreateAssetFromAsset(3, "c", 1);
        List<Asset> assets = new() { asset1, asset2, asset3 };

        // When
        var spec = new AssetsByParentIdSpec(1);
        var filteredAssets = spec.Evaluate(assets);

        // Then
        Assert.That(
            filteredAssets,
            Is.EquivalentTo(new List<Asset>() { asset1, asset3 }),
            nameof(filteredAssets)
        );
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithInvalidParentId()
    {
        // Given
        var asset1 = AssetFixture.CreateAssetFromAsset(1, "a", 1);
        var asset2 = AssetFixture.CreateAssetFromAsset(2, "b", 1);
        var asset3 = AssetFixture.CreateAssetFromAsset(3, "c", 1);
        List<Asset> assets = new() { asset1, asset2, asset3 };

        // When
        var spec = new AssetsByParentIdSpec(100);
        var filteredAssets = spec.Evaluate(assets);

        // Then
        Assert.That(filteredAssets, Is.Empty, nameof(filteredAssets));
    }
}
