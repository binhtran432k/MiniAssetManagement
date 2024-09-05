using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.UserAggregate;

public class User_UpdateUsername
{
    [Test]
    public void UpdatesUsername()
    {
        // Given
        User user = new(UserFixture.UsernameDefault);

        // When
        user.UpdateUsername(UserFixture.UsernameNew);

        // Then
        Assert.That(user.Username, Is.EqualTo(UserFixture.UsernameNew), nameof(user.Username));
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyUsername()
    {
        User user = new(UserFixture.UsernameDefault);

        Assert.Throws<ArgumentNullException>(() => user.UpdateUsername(null!), nameof(user));
        Assert.Throws<ArgumentException>(() => user.UpdateUsername(""), nameof(user));
    }
}