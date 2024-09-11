using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.Infrastructure.Data.Config;

public class FolderConfiguration : IEntityTypeConfiguration<Folder>
{
    public void Configure(EntityTypeBuilder<Folder> builder)
    {
        builder
            .Property(f => f.Name)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .IsRequired();

        builder.HasOne(f => f.Drive).WithMany(d => d.Folders).HasForeignKey(f => f.DriveId);

        builder.HasOne(f => f.Parent).WithMany(p => p.Children).HasForeignKey(f => f.ParentId);

        builder.HasMany(f => f.Permissions).WithOne(p => p.Folder).HasForeignKey(p => p.FolderId);

        builder.Property(f => f.Status).HasConversion(s => s.Value, s => FolderStatus.FromValue(s));
    }
}
