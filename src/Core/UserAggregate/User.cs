using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.Core.UserAggregate;

public class User(string username) : EntityBase, IAggregateRoot
{
    public string Username { get; private set; } =
        Guard.Against.NullOrEmpty(username, nameof(username));
    public UserStatus Status { get; private set; } = UserStatus.Available;

    public void UpdateUsername(string newUsername) =>
        Username = Guard.Against.NullOrEmpty(newUsername, nameof(newUsername));

    public void UpdateStatus(UserStatus newStatus) => Status = newStatus;
}