using Ardalis.SmartEnum;

namespace MiniAssetManagement.Core.FolderAggregate;

public class PermissionType : SmartEnum<PermissionType>
{
    public static readonly PermissionType Admin = new(nameof(Admin), 1);
    public static readonly PermissionType Contributor = new(nameof(Contributor), 2);
    public static readonly PermissionType Reader = new(nameof(Reader), 3);

    public PermissionType(string name, int value)
        : base(name, value) { }
}
