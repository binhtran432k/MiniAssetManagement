using Ardalis.SmartEnum;

namespace MiniAssetManagement.Core.DriveAggregate;

public class DriveStatus : SmartEnum<DriveStatus>
{
    public static readonly DriveStatus Available = new(nameof(Available), 1);
    public static readonly DriveStatus Deleted = new(nameof(Deleted), 2);

    public DriveStatus(string name, int value)
        : base(name, value) { }
}
