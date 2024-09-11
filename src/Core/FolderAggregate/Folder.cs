using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;

namespace MiniAssetManagement.Core.FolderAggregate;

public class Folder : EntityBase, IAggregateRoot
{
    public int? DriveId { get; private set; }
    public int? ParentId { get; private set; }
    public string Name { get; private set; }
    public FolderStatus Status { get; private set; } = FolderStatus.Available;

    public Drive? Drive { get; set; }
    public Folder? Parent { get; set; }
    public IEnumerable<Folder> Children => _children.AsReadOnly();
    public IEnumerable<Permission> Permissions => _permissions.AsReadOnly();

    public readonly List<Folder> _children = new();
    private readonly List<Permission> _permissions = new();

    private Folder(string name) => Name = Guard.Against.NullOrEmpty(name, nameof(name));

    public static Folder CreateFromDrive(string name, int driveId) =>
        new(name) { DriveId = driveId };

    public static Folder CreateFromFolder(string name, int folderId) =>
        new(name) { ParentId = folderId };

    public void UpdateName(string newName) =>
        Name = Guard.Against.NullOrEmpty(newName, nameof(newName));

    public void UpdateStatus(FolderStatus newStatus) => Status = newStatus;

    public void AddOrUpdatePermission(int userId, PermissionType type)
    {
        var permission = _permissions.Find(p => p.UserId == userId);
        if (permission is null)
            _permissions.Add(new(Id, userId, type));
        else
            permission.SetType(type);
    }

    public void RemovePermissionByUserId(int userId) =>
        _permissions.RemoveAll(p => p.UserId == userId);
}
