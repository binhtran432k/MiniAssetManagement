using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.UserAggregate;

public class User_UpdateStatus
{
    [Test]
    public void UpdatesStatus()
    {
        // Given
        User user = new(UserFixture.UsernameDefault);

        // When
        user.UpdateStatus(UserStatus.Deleted);

        // Then
        Assert.That(user.Status, Is.EqualTo(UserStatus.Deleted), nameof(user.Status));
    }
}
