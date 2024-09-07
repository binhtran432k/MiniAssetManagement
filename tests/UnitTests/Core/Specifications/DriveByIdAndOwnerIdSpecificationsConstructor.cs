using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.Specifications;

public class DriveByIdAndOwnerIdSpecificationsConstructor
{
    [Test]
    public void FilterCollectionToOnlyReturnItemsWithValidId()
    {
        // Given
        var drive1 = DriveFixture.CreateDrive(1, "a", 1);
        var drive2 = DriveFixture.CreateDrive(2, "b", 1);
        var drive3 = DriveFixture.CreateDrive(3, "c", 1);
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        DriveByIdAndOwnerIdSpec spec = new(1, 1);
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
        var drive1 = DriveFixture.CreateDrive(1, "a", 1);
        var drive2 = DriveFixture.CreateDrive(2, "b", 1);
        var drive3 = DriveFixture.CreateDrive(3, "c", 1);
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        DriveByIdAndOwnerIdSpec spec = new(100, 1);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(filteredDrives, Is.Empty, nameof(filteredDrives));
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
        DriveByIdAndOwnerIdSpec spec = new(1, 2);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(filteredDrives, Is.Empty, nameof(filteredDrives));
    }

    [Test]
    public void FilterCollectionToReturnEmptyWithDeletedId()
    {
        // Given
        var drive1 = DriveFixture.CreateDrive(1, "a", 1);
        var drive2 = DriveFixture.CreateDrive(2, "b", 1, DriveStatus.Deleted);
        var drive3 = DriveFixture.CreateDrive(3, "c", 1);
        List<Drive> drives = new() { drive1, drive2, drive3 };

        // When
        DriveByIdAndOwnerIdSpec spec = new(2, 1);
        var filteredDrives = spec.Evaluate(drives);

        // Then
        Assert.That(filteredDrives, Is.Empty, nameof(filteredDrives));
    }
}
