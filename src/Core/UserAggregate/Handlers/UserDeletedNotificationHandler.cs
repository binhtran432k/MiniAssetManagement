using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.UserAggregate.Events;

namespace MiniAssetManagement.Core.UserAggregate.Handlers;

public class UserDeletedNotificationHandler : INotificationHandler<UserDeletedEvent>
{
    private readonly ILogger<UserDeletedNotificationHandler> _logger;

    public UserDeletedNotificationHandler(ILogger<UserDeletedNotificationHandler> logger) =>
        _logger = logger;

    public Task Handle(UserDeletedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling deleted user {userId}...", domainEvent.UserId);
        return Task.CompletedTask;
    }
}
