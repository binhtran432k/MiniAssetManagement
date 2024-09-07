using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class DrivesByOwnerIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidOwnerId()
    {
        // Given
        var drive1 = DriveFixture.CreateDrive(1, "a", 1);
        var drive2 = DriveFixture.CreateDrive(2, "b", 2);
        var drive3 = DriveFixture.CreateDrive(3, "c", 1);
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
        var drive1 = DriveFixture.CreateDrive(1, "a", 1);
        var drive2 = DriveFixture.CreateDrive(2, "b", 1);
        var drive3 = DriveFixture.CreateDrive(3, "c", 1);
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        var spec = new DrivesByOwnerIdSpec(100);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(filteredDrives, Is.Empty, nameof(filteredDrives));
    }
}