using Ardalis.SharedKernel;

namespace MiniAssetManagement.Core.UserAggregate.Events;

public class UserDeletedEvent(int userId) : DomainEventBase
{
    public int UserId { get; init; } = userId;
}
