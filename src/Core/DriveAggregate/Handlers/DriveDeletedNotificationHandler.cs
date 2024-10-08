using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.DriveAggregate.Events;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Events;
using MiniAssetManagement.Core.AssetAggregate.Specifications;

namespace MiniAssetManagement.Core.DriveAggregate.Handlers;

public class DriveDeletedNotificationHandler(
    ILogger<DriveDeletedEvent> logger,
    IMediator mediator,
    IRepository<Asset> assetRepository
) : INotificationHandler<DriveDeletedEvent>
{
    private readonly ILogger<DriveDeletedEvent> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly IRepository<Asset> _assetRepository = assetRepository;

    public async Task Handle(DriveDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting assets with drive {driveId}...", domainEvent.DriveId);
        var assets = await _assetRepository.ListAsync(
            new AssetsByDriveIdSpec(domainEvent.DriveId),
            cancellationToken
        );
        foreach (var asset in assets)
        {
            asset.UpdateStatus(AssetStatus.Deleted);
            await _assetRepository.UpdateAsync(asset, cancellationToken);
            await _mediator.Publish(new AssetDeletedEvent(asset.Id));
        }
    }
}
