using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniAssetManagement.Core.AssetAggregate;

namespace MiniAssetManagement.Infrastructure.Data.Config;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => new { p.AssetId, p.UserId });

        builder.Property(p => p.Type).HasConversion(s => s.Value, t => PermissionType.FromValue(t));
    }
}
