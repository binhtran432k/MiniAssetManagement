using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.DriveAggregate;

public class Drive_UpdateName
{
    [Test]
    public void UpdatesName()
    {
        // Given
        var user = UserFixture.CreateUserDefault();
        Drive drive = new(DriveFixture.NameDefault, user.Id);

        // When
        drive.UpdateName(DriveFixture.NameNew);

        // Then
        Assert.That(drive.Name, Is.EqualTo(DriveFixture.NameNew), nameof(drive.Name));
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyName()
    {
        Drive drive = new(DriveFixture.NameDefault, 1);

        Assert.Throws<ArgumentNullException>(() => drive.UpdateName(null!), nameof(drive));
        Assert.Throws<ArgumentException>(() => drive.UpdateName(""), nameof(drive));
    }
}