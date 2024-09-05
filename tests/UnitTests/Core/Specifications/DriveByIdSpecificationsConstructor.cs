using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class DriveByIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidId()
    {
        // Given
        Drive drive1 = new("a", 1) { Id = 1 };
        Drive drive2 = new("b", 1) { Id = 2 };
        Drive drive3 = new("c", 1) { Id = 3 };
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        var spec = new DriveByIdSpec(1);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(
            filteredDrives,
            Is.EquivalentTo(new List<Drive>() { drive1 }),
            nameof(filteredDrives)
        );
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithInvalidId()
    {
        // Given
        Drive drive1 = new("a", 1) { Id = 1 };
        Drive drive2 = new("b", 1) { Id = 2 };
        Drive drive3 = new("c", 1) { Id = 3 };
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        var spec = new DriveByIdSpec(100);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(filteredDrives, Is.Empty, nameof(filteredDrives));
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithDeletedId()
    {
        // Given
        Drive drive1 = new("a", 1) { Id = 1 };
        Drive drive2 = new("b", 1) { Id = 2 };
        drive2.UpdateStatus(DriveStatus.Deleted);
        Drive drive3 = new("c", 1) { Id = 3 };
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        var spec = new DriveByIdSpec(2);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(filteredDrives, Is.Empty, nameof(filteredDrives));
    }
}
