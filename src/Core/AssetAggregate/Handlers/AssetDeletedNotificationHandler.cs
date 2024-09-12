using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.AssetAggregate.Events;
using MiniAssetManagement.Core.AssetAggregate.Specifications;

namespace MiniAssetManagement.Core.AssetAggregate.Handlers;

public class AssetDeletedNotificationHandler(
    ILogger<AssetDeletedEvent> logger,
    IMediator mediator,
    IRepository<Asset> assetRepository
) : INotificationHandler<AssetDeletedEvent>
{
    private readonly ILogger<AssetDeletedEvent> _logger = logger;
    private readonly IMediator _mediator = mediator;
    private readonly IRepository<Asset> _assetRepository = assetRepository;

    public async Task Handle(AssetDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting assets with parent {assetId}...", domainEvent.AssetId);
        var assets = await _assetRepository.ListAsync(
            new AssetsByParentIdSpec(domainEvent.AssetId),
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
