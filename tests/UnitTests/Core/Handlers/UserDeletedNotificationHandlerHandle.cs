using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Events;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.Core.UserAggregate.Events;
using MiniAssetManagement.Core.UserAggregate.Handlers;
using MiniAssetManagement.UnitTests.Fixtures;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.Core.Handlers;

public class UserDeletedNotificationHandlerHandle
{
    private ILogger<UserDeletedEvent> _logger = Substitute.For<ILogger<UserDeletedEvent>>();
    private IMediator _mediator = Substitute.For<IMediator>();
    private IRepository<Drive> _driveRepository = Substitute.For<IRepository<Drive>>();
    private UserDeletedNotificationHandler _handler;

    public UserDeletedNotificationHandlerHandle() =>
        _handler = new(_logger, _mediator, _driveRepository);

    [Test]
    public async Task DeleteDrivesMatchOwnerId()
    {
        // Given
        Drive drive1 = new("a", 1);
        Drive drive2 = new("b", 1);
        List<Drive> drives = new() { drive1, drive2 };
        _driveRepository
            .ListAsync(Arg.Any<DrivesByOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(drives);

        // When
        await _handler.Handle(new(UserFixture.IdToDelete), CancellationToken.None);

        // Then
        await _driveRepository
            .Received(2)
            .UpdateAsync(Arg.Any<Drive>(), Arg.Any<CancellationToken>());
        await _mediator
            .Received(2)
            .Publish(Arg.Any<DriveDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.Multiple(() =>
        {
            Assert.That(drive1.Status, Is.EqualTo(DriveStatus.Deleted), nameof(drive1.Status));
            Assert.That(drive2.Status, Is.EqualTo(DriveStatus.Deleted), nameof(drive2.Status));
        });
    }
}
