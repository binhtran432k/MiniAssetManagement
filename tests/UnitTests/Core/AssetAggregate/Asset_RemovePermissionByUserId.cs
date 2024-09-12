using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.AssetAggregate;

public class Asset_RemoveAssetPermissionByUserId
{
    [Test]
    public void RemovesPermissionSuccess()
    {
        // Given
        Asset asset = Asset.CreateFromDrive(AssetFixture.NameDefault, AssetFixture.DriveIdDefault);
        asset.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);

        // When
        asset.RemovePermissionByUserId(UserFixture.IdDefault);

        // Then
        Assert.That(asset.Permissions, Is.Empty, nameof(asset.Permissions));
    }
}
