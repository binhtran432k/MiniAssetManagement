using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class FoldersByParentIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidParentId()
    {
        // Given
        var folder1 = FolderFixture.CreateFolderFromFolder(1, "a", 1);
        var folder2 = FolderFixture.CreateFolderFromFolder(2, "b", 2);
        var folder3 = FolderFixture.CreateFolderFromFolder(3, "c", 1);
        List<Folder> folders = new() { folder1, folder2, folder3 };

        // When
        var spec = new FoldersByParentIdSpec(1);
        var filteredFolders = spec.Evaluate(folders);

        // Then
        Assert.That(
            filteredFolders,
            Is.EquivalentTo(new List<Folder>() { folder1, folder3 }),
            nameof(filteredFolders)
        );
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithInvalidParentId()
    {
        // Given
        var folder1 = FolderFixture.CreateFolderFromFolder(1, "a", 1);
        var folder2 = FolderFixture.CreateFolderFromFolder(2, "b", 1);
        var folder3 = FolderFixture.CreateFolderFromFolder(3, "c", 1);
        List<Folder> folders = new() { folder1, folder2, folder3 };

        // When
        var spec = new FoldersByParentIdSpec(100);
        var filteredFolders = spec.Evaluate(folders);

        // Then
        Assert.That(filteredFolders, Is.Empty, nameof(filteredFolders));
    }
}
