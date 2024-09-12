using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class AssetWithPermissionsByIdSpecificationsConstructor
{
    [Test]
    public void FiltersSuccess()
    {
        // Given
        var folder1 = AssetFixture.CreateFolderFromDrive(1, "a", 1);
        var folder2 = AssetFixture.CreateFolderFromDrive(2, "b", 1);
        var folder3 = AssetFixture.CreateFolderFromDrive(3, "c", 1);
        List<Asset> folders = new() { folder1, folder2, folder3 };

        // When
        AssetWithPermissionsByIdSpec spec = new(1);
        var filteredAssets = spec.Evaluate(folders);

        // Then
        Assert.That(
            filteredAssets,
            Is.EquivalentTo(new List<Asset>() { folder1 }),
            nameof(filteredAssets)
        );
    }

    [Test]
    public void FiltersEmptyByInvalidId()
    {
        // Given
        var folder1 = AssetFixture.CreateFolderFromDrive(1, "a", 1);
        var folder2 = AssetFixture.CreateFolderFromDrive(2, "b", 1);
        var folder3 = AssetFixture.CreateFolderFromDrive(3, "c", 1);
        List<Asset> folders = new() { folder1, folder2, folder3 };

        // When
        AssetWithPermissionsByIdSpec spec = new(100);
        var filteredAssets = spec.Evaluate(folders);

        // Then
        Assert.That(filteredAssets, Is.Empty, nameof(filteredAssets));
    }

    [Test]
    public void FiltersEmptyByDeletedId()
    {
        // Given
        var folder1 = AssetFixture.CreateFolderFromDrive(1, "a", 1);
        var folder2 = AssetFixture.CreateFolderFromDrive(2, "b", 1, AssetStatus.Deleted);
        var folder3 = AssetFixture.CreateFolderFromDrive(3, "c", 1);
        List<Asset> folders = new() { folder1, folder2, folder3 };

        // When
        AssetWithPermissionsByIdSpec spec = new(2);
        var filteredAssets = spec.Evaluate(folders);

        // Then
        Assert.That(filteredAssets, Is.Empty, nameof(filteredAssets));
    }
}
