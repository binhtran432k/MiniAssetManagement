using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.Core.DriveAggregate;

public class Drive(string name, int ownerId) : EntityBase, IAggregateRoot
{
    public int OwnerId { get; private set; } = ownerId;
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name, nameof(name));
    public DriveStatus Status { get; private set; } = DriveStatus.Available;

    public IEnumerable<Asset> Assets => _assets.AsReadOnly();
    public readonly List<Asset> _assets = new();

    public User Owner { get; set; } = default!;

    public void UpdateName(string newName) =>
        Name = Guard.Against.NullOrEmpty(newName, nameof(newName));

    public void UpdateStatus(DriveStatus newStatus) => Status = newStatus;
}
