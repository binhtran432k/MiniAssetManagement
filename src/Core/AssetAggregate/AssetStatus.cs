using Ardalis.SmartEnum;

namespace MiniAssetManagement.Core.AssetAggregate;

public class AssetStatus : SmartEnum<AssetStatus>
{
    public static readonly AssetStatus Available = new(nameof(Available), 1);
    public static readonly AssetStatus Deleted = new(nameof(Deleted), 2);

    public AssetStatus(string name, int value)
        : base(name, value) { }
}
