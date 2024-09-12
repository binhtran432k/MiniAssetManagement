using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class AssetByIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidId()
    {
        // Given
        var asset1 = AssetFixture.CreateFolderFromDrive(1, "a", 1);
        var asset2 = AssetFixture.CreateFolderFromDrive(2, "b", 1);
        var asset3 = AssetFixture.CreateFolderFromDrive(3, "c", 1);
        List<Asset> assets = new() { asset1, asset2, asset3 };

        // When
        var spec = new AssetByIdSpec(1);
        var filteredAssets = spec.Evaluate(assets);

        // Then
        Assert.That(
            filteredAssets,
            Is.EquivalentTo(new List<Asset>() { asset1 }),
            nameof(filteredAssets)
        );
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithInvalidId()
    {
        // Given
        var asset1 = AssetFixture.CreateFolderFromDrive(1, "a", 1);
        var asset2 = AssetFixture.CreateFolderFromDrive(2, "b", 1);
        var asset3 = AssetFixture.CreateFolderFromDrive(3, "c", 1);
        List<Asset> assets = new() { asset1, asset2, asset3 };

        // When
        var spec = new AssetByIdSpec(100);
        var filteredAssets = spec.Evaluate(assets);

        // Then
        Assert.That(filteredAssets, Is.Empty, nameof(filteredAssets));
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithDeletedId()
    {
        // Given
        var asset1 = AssetFixture.CreateFolderFromDrive(1, "a", 1);
        var asset2 = AssetFixture.CreateFolderFromDrive(2, "b", 1, AssetStatus.Deleted);
        var asset3 = AssetFixture.CreateFolderFromDrive(3, "c", 1);
        List<Asset> assets = new() { asset1, asset2, asset3 };

        // When
        var spec = new AssetByIdSpec(2);
        var filteredAssets = spec.Evaluate(assets);

        // Then
        Assert.That(filteredAssets, Is.Empty, nameof(filteredAssets));
    }
}
