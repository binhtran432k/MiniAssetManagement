using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.DriveAggregate.Events;
using MiniAssetManagement.Core.DriveAggregate.Handlers;
using MiniAssetManagement.UnitTests.Fixtures;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.Core.Handlers;

public class DriveDeletedNotificationHandlerHandle
{
    private ILogger<DriveDeletedEvent> _logger = Substitute.For<ILogger<DriveDeletedEvent>>();
    private DriveDeletedNotificationHandler _handler;

    public DriveDeletedNotificationHandlerHandle() => _handler = new(_logger);

    [Test]
    public void NotThrowsExceptionGivenValidEventArgument()
    {
        Assert.DoesNotThrowAsync(
            () => _handler.Handle(new(DriveFixture.IdToDelete), CancellationToken.None),
            nameof(_handler)
        );
    }
}
