using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniAssetManagement.Core.AssetAggregate;

namespace MiniAssetManagement.Infrastructure.Data.Config;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder
            .Property(f => f.Name)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .IsRequired();

        builder.HasOne(f => f.Drive).WithMany(d => d.Assets).HasForeignKey(f => f.DriveId);

        builder.HasOne(f => f.Parent).WithMany(p => p.Children).HasForeignKey(f => f.ParentId);

        builder.HasMany(f => f.Permissions).WithOne(p => p.Asset).HasForeignKey(p => p.AssetId);

        builder.Property(f => f.Status).HasConversion(s => s.Value, s => AssetStatus.FromValue(s));
    }
}
