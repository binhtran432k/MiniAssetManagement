using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.Core.DriveAggregate;

public class Drive(string name, int ownerId) : EntityBase, IAggregateRoot
{
    public int OwnerId { get; private set; } = ownerId;
    public string Name { get; private set; } = Guard.Against.NullOrEmpty(name, nameof(name));
    public DriveStatus Status { get; private set; } = DriveStatus.Available;

    public void UpdateName(string newName) =>
        Name = Guard.Against.NullOrEmpty(newName, nameof(newName));

    public void UpdateStatus(DriveStatus newStatus) => Status = newStatus;
}
