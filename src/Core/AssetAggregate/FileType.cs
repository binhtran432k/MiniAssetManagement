using Ardalis.SmartEnum;

namespace MiniAssetManagement.Core.AssetAggregate;

public class FileType : SmartEnum<FileType>
{
    public static readonly FileType Text = new(nameof(Text), 1);
    public static readonly FileType Document = new(nameof(Document), 2);
    public static readonly FileType PDF = new(nameof(PDF), 3);
    public static readonly FileType Unknown = new(nameof(Unknown), 4);

    public FileType(string name, int value)
        : base(name, value) { }
}
