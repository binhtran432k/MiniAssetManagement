using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.DriveAggregate.Events;

namespace MiniAssetManagement.Core.DriveAggregate.Handlers;

public class DriveDeletedNotificationHandler : INotificationHandler<DriveDeletedEvent>
{
    private readonly ILogger<DriveDeletedEvent> _logger;

    public DriveDeletedNotificationHandler(ILogger<DriveDeletedEvent> logger) => _logger = logger;

    public Task Handle(DriveDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling deleted drive {driveId}...", domainEvent.DriveId);
        return Task.CompletedTask;
    }
}
