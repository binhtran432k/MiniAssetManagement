using Ardalis.SmartEnum;

namespace MiniAssetManagement.Core.UserAggregate;

public class UserStatus : SmartEnum<UserStatus>
{
    public static readonly UserStatus Available = new(nameof(Available), 1);
    public static readonly UserStatus Deleted = new(nameof(Deleted), 2);

    public UserStatus(string name, int value)
        : base(name, value) { }
}
