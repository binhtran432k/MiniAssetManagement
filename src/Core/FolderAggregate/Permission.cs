using Ardalis.SharedKernel;
using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.Core.FolderAggregate;

public class Permission(int folderId, int userId, PermissionType type) : ValueObject
{
    public int FolderId { get; private set; } = folderId;
    public int UserId { get; private set; } = userId;
    public PermissionType Type { get; private set; } = type;

    public Folder Folder { get; set; } = null!;
    public User User { get; set; } = null!;

    public void SetType(PermissionType type) => Type = type;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FolderId;
        yield return UserId;
        yield return Type;
    }

    public override string ToString() => String.Format("{0}:{1}:{2}", FolderId, UserId, Type);
}
