using Ardalis.SmartEnum;

namespace MiniAssetManagement.Core.FolderAggregate;

public class FolderStatus : SmartEnum<FolderStatus>
{
    public static readonly FolderStatus Available = new(nameof(Available), 1);
    public static readonly FolderStatus Deleted = new(nameof(Deleted), 2);

    public FolderStatus(string name, int value)
        : base(name, value) { }
}
