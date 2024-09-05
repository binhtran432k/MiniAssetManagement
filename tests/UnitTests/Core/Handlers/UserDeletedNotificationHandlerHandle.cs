using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.UserAggregate.Handlers;
using MiniAssetManagement.UnitTests.Fixtures;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.Core.Handlers;

public class UserDeletedNotificationHandlerHandle
{
    private ILogger<UserDeletedNotificationHandler> _logger = Substitute.For<
        ILogger<UserDeletedNotificationHandler>
    >();
    private UserDeletedNotificationHandler _handler;

    public UserDeletedNotificationHandlerHandle() => _handler = new(_logger);

    [Test]
    public void NotThrowsExceptionGivenValidEventArgument()
    {
        Assert.DoesNotThrowAsync(
            () => _handler.Handle(new(UserFixture.IdToDelete), CancellationToken.None),
            nameof(_handler)
        );
    }
}
