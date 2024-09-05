using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Events;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.Core.UserAggregate.Events;

namespace MiniAssetManagement.Core.UserAggregate.Handlers;

public class UserDeletedNotificationHandler(
    ILogger<UserDeletedEvent> logger,
    IMediator mediator,
    IRepository<Drive> driveRepository
) : INotificationHandler<UserDeletedEvent>
{
    private readonly ILogger<UserDeletedEvent> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly IRepository<Drive> _driveRepository = driveRepository;

    public async Task Handle(UserDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting drives with owner {userId}...", domainEvent.UserId);
        var drives = await _driveRepository.ListAsync(
            new DrivesByOwnerIdSpec(domainEvent.UserId),
            cancellationToken
        );
        foreach (var drive in drives)
        {
            drive.UpdateStatus(DriveStatus.Deleted);
            await _driveRepository.UpdateAsync(drive, cancellationToken);
            await _mediator.Publish(new DriveDeletedEvent(drive.Id));
        }
    }
}
