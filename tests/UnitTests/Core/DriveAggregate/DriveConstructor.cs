using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.DriveAggregate;

public class DriveConstructor
{
    [Test]
    public void InitializesDrive()
    {
        // When
        Drive drive = new(DriveFixture.NameDefault, 1);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(drive.Name, Is.EqualTo(DriveFixture.NameDefault), nameof(drive.Name));
            Assert.That(drive.Status, Is.EqualTo(DriveStatus.Available), nameof(drive.Status));
        });
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyName()
    {
        Assert.Throws<ArgumentNullException>(() => new Drive(null!, 1), nameof(Drive));
        Assert.Throws<ArgumentException>(() => new Drive("", 1), nameof(Drive));
    }
}