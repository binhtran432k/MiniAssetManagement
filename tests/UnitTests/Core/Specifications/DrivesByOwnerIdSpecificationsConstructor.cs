using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class DrivesByOwnerIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidOwnerId()
    {
        // Given
        Drive drive1 = new("a", 1) { Id = 1 };
        Drive drive2 = new("b", 2) { Id = 2 };
        Drive drive3 = new("c", 1) { Id = 3 };
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        var spec = new DrivesByOwnerIdSpec(1);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(
            filteredDrives,
            Is.EquivalentTo(new List<Drive>() { drive1, drive3 }),
            nameof(filteredDrives)
        );
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithInvalidOwnerId()
    {
        // Given
        Drive drive1 = new("a", 1) { Id = 1 };
        Drive drive2 = new("b", 1) { Id = 2 };
        Drive drive3 = new("c", 1) { Id = 3 };
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        var spec = new DrivesByOwnerIdSpec(100);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(filteredDrives, Is.Empty, nameof(filteredDrives));
    }
}
