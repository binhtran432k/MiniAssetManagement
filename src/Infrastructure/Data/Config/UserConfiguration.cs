using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniAssetManagement.Core.UserAggregate;

namespace MiniAssetManagement.Infrastructure.Data.Config;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(u => u.Username)
            .HasMaxLength(DataSchemaConstants.DEFAULT_USERNAME_LENGTH)
            .IsRequired();

        builder.HasMany(u => u.Permissions).WithOne(p => p.User).HasForeignKey(p => p.UserId);

        builder.Property(u => u.Status).HasConversion(s => s.Value, s => UserStatus.FromValue(s));
    }
}