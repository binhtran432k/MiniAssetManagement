using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.DriveAggregate.Events;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Events;
using MiniAssetManagement.Core.FolderAggregate.Specifications;

namespace MiniAssetManagement.Core.DriveAggregate.Handlers;

public class DriveDeletedNotificationHandler(
    ILogger<DriveDeletedEvent> logger,
    IMediator mediator,
    IRepository<Folder> folderRepository
) : INotificationHandler<DriveDeletedEvent>
{
    private readonly ILogger<DriveDeletedEvent> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly IRepository<Folder> _folderRepository = folderRepository;

    public async Task Handle(DriveDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting folders with drive {driveId}...", domainEvent.DriveId);
        var folders = await _folderRepository.ListAsync(
            new FoldersByDriveIdSpec(domainEvent.DriveId),
            cancellationToken
        );
        foreach (var folder in folders)
        {
            folder.UpdateStatus(FolderStatus.Deleted);
            await _folderRepository.UpdateAsync(folder, cancellationToken);
            await _mediator.Publish(new FolderDeletedEvent(folder.Id));
        }
    }
}
