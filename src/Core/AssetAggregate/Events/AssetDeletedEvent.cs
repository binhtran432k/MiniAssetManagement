using Ardalis.SharedKernel;

namespace MiniAssetManagement.Core.AssetAggregate.Events;

public class AssetDeletedEvent(int assetId) : DomainEventBase
{
    public int AssetId { get; init; } = assetId;
}
