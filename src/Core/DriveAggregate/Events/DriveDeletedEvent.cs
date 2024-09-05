using Ardalis.SharedKernel;

namespace MiniAssetManagement.Core.DriveAggregate.Events;

public class DriveDeletedEvent : DomainEventBase
{
    public int DriveId { get; set; }

    public DriveDeletedEvent(int driveId) => DriveId = driveId;
}