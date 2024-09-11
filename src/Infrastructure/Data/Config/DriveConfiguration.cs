using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniAssetManagement.Core.DriveAggregate;

namespace MiniAssetManagement.Infrastructure.Data.Config;

public class DriveConfiguration : IEntityTypeConfiguration<Drive>
{
    public void Configure(EntityTypeBuilder<Drive> builder)
    {
        builder
            .HasOne(d => d.Owner)
            .WithMany(o => o.Drives)
            .HasForeignKey(d => d.OwnerId)
            .IsRequired();

        builder
            .Property(d => d.Name)
            .HasMaxLength(DataSchemaConstants.DEFAULT_NAME_LENGTH)
            .IsRequired();

        builder.Property(d => d.Status).HasConversion(s => s.Value, s => DriveStatus.FromValue(s));
    }
}
