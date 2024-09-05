using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.UserAggregate;

public class UserConstructor
{
    [Test]
    public void InitializesUser()
    {
        // When
        User user = new(UserFixture.UsernameDefault);

        // Then
        Assert.Multiple(() =>
        {
            Assert.That(
                user.Username,
                Is.EqualTo(UserFixture.UsernameDefault),
                nameof(user.Username)
            );
            Assert.That(user.Status, Is.EqualTo(UserStatus.Available), nameof(user.Status));
        });
    }

    [Test]
    public void ThrowsExceptionGivenNullOrEmptyUsername()
    {
        Assert.Throws<ArgumentNullException>(() => new User(null!), nameof(User));
        Assert.Throws<ArgumentException>(() => new User(""), nameof(User));
    }
}
