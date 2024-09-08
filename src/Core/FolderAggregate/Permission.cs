using Ardalis.SharedKernel;

namespace MiniAssetManagement.Core.FolderAggregate;

public class Permission(int userId, PermissionType type) : ValueObject
{
    public int UserId { get; private set; } = userId;
    public PermissionType Type { get; private set; } = type;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return Type;
    }

    public void SetType(PermissionType type) => Type = type;
}