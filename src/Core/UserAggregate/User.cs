using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.AssetAggregate;

namespace MiniAssetManagement.Core.UserAggregate;

public class User(string username) : EntityBase, IAggregateRoot
{
    public string Username { get; private set; } =
        Guard.Against.NullOrEmpty(username, nameof(username));
    public UserStatus Status { get; private set; } = UserStatus.Available;
    public IEnumerable<Permission> Permissions => _permissions.AsReadOnly();
    public IEnumerable<Drive> Drives => _drives.AsReadOnly();

    private readonly List<Permission> _permissions = new();
    private readonly List<Drive> _drives = new();

    public void UpdateUsername(string newUsername) =>
        Username = Guard.Against.NullOrEmpty(newUsername, nameof(newUsername));

    public void UpdateStatus(UserStatus newStatus) => Status = newStatus;
}
