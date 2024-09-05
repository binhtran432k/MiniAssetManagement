using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.DriveAggregate;

public class Drive_UpdateStatus
{
    [Test]
    public void UpdatesStatus()
    {
        // Given
        Drive drive = new(DriveFixture.NameDefault, 1);

        // When
        drive.UpdateStatus(DriveStatus.Deleted);

        // Then
        Assert.That(drive.Status, Is.EqualTo(DriveStatus.Deleted), nameof(drive.Status));
    }
}