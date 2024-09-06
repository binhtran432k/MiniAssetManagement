using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.FolderAggregate.Events;
using MiniAssetManagement.Core.FolderAggregate.Specifications;

namespace MiniAssetManagement.Core.FolderAggregate.Handlers;

public class FolderDeletedNotificationHandler(
    ILogger<FolderDeletedEvent> logger,
    IMediator mediator,
    IRepository<Folder> folderRepository
) : INotificationHandler<FolderDeletedEvent>
{
    private readonly ILogger<FolderDeletedEvent> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly IRepository<Folder> _folderRepository = folderRepository;

    public async Task Handle(FolderDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting folders with parent {folderId}...", domainEvent.FolderId);
        var folders = await _folderRepository.ListAsync(
            new FoldersByParentIdSpec(domainEvent.FolderId),
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
