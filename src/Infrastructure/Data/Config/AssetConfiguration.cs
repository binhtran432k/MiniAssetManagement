using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniAssetManagement.Core.AssetAggregate;

namespace MiniAssetManagement.Infrastructure.Data.Config;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder
            .Property(a => a.Name)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .IsRequired();

        builder.HasOne(a => a.Drive).WithMany(d => d.Assets).HasForeignKey(f => f.DriveId);

        builder.HasOne(a => a.Parent).WithMany(p => p.Children).HasForeignKey(f => f.ParentId);

        builder.HasMany(a => a.Permissions).WithOne(p => p.Asset).HasForeignKey(p => p.AssetId);

        builder.Property(a => a.Status).HasConversion(s => s.Value, s => AssetStatus.FromValue(s));

        builder
            .Property(a => a.FileType)
            .HasConversion(
                t => t == null ? (int?)null : t.Value,
                t => t == null ? null : FileType.FromValue((int)t)
            );
    }
}
