using Ardalis.SharedKernel;
using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.Core.AssetAggregate;

public class Permission(int assetId, int userId, PermissionType type) : ValueObject
{
    public int AssetId { get; private set; } = assetId;
    public int UserId { get; private set; } = userId;
    public PermissionType Type { get; private set; } = type;

    public Asset Asset { get; set; } = null!;
    public User User { get; set; } = null!;

    public void SetType(PermissionType type) => Type = type;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return AssetId;
        yield return UserId;
        yield return Type;
    }

    public override string ToString() => String.Format("{0}:{1}:{2}", AssetId, UserId, Type);
}
