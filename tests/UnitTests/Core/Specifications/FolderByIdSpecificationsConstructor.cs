using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class FolderByIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidId()
    {
        // Given
        var folder1 = FolderFixture.CreateFolderFromDrive(1, "a", 1);
        var folder2 = FolderFixture.CreateFolderFromDrive(2, "b", 1);
        var folder3 = FolderFixture.CreateFolderFromDrive(3, "c", 1);
        List<Folder> folders = new() { folder1, folder2, folder3 };

        // When
        var spec = new FolderByIdSpec(1);
        var filteredFolders = spec.Evaluate(folders);

        // Then
        Assert.That(
            filteredFolders,
            Is.EquivalentTo(new List<Folder>() { folder1 }),
            nameof(filteredFolders)
        );
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithInvalidId()
    {
        // Given
        var folder1 = FolderFixture.CreateFolderFromDrive(1, "a", 1);
        var folder2 = FolderFixture.CreateFolderFromDrive(2, "b", 1);
        var folder3 = FolderFixture.CreateFolderFromDrive(3, "c", 1);
        List<Folder> folders = new() { folder1, folder2, folder3 };

        // When
        var spec = new FolderByIdSpec(100);
        var filteredFolders = spec.Evaluate(folders);

        // Then
        Assert.That(filteredFolders, Is.Empty, nameof(filteredFolders));
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithDeletedId()
    {
        // Given
        var folder1 = FolderFixture.CreateFolderFromDrive(1, "a", 1);
        var folder2 = FolderFixture.CreateFolderFromDrive(2, "b", 1, FolderStatus.Deleted);
        var folder3 = FolderFixture.CreateFolderFromDrive(3, "c", 1);
        List<Folder> folders = new() { folder1, folder2, folder3 };

        // When
        var spec = new FolderByIdSpec(2);
        var filteredFolders = spec.Evaluate(folders);

        // Then
        Assert.That(filteredFolders, Is.Empty, nameof(filteredFolders));
    }
}
