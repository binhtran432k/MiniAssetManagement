using Ardalis.GuardClauses;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.Core.FolderAggregate;

public class Folder : EntityBase, IAggregateRoot
{
    public int? DriveId { get; private set; }
    public int? ParentId { get; private set; }
    public string Name { get; private set; }
    public FolderStatus Status { get; private set; } = FolderStatus.Available;

    public IEnumerable<Permission> Permissions => _permissions.AsReadOnly();

    private readonly List<Permission> _permissions = new();

    private Folder(string name) => Name = Guard.Against.NullOrEmpty(name, nameof(name));

    public static Folder CreateFromDrive(string name, int driveId) =>
        new(name) { DriveId = driveId };

    public static Folder CreateFromFolder(string name, int folderId) =>
        new(name) { ParentId = folderId };

    public void UpdateName(string newName) =>
        Name = Guard.Against.NullOrEmpty(newName, nameof(newName));

    public void UpdateStatus(FolderStatus newStatus) => Status = newStatus;

    public void AddPermission(Permission permission) => _permissions.Add(permission);
}