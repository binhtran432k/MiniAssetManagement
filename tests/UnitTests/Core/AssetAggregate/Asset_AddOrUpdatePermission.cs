using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;

namespace MiniAssetManagement.UnitTests.Core.AssetAggregate;

public class Asset_AddOrUpdateAssetPermission
{
    [Test]
    public void AddsPermissionSuccess()
    {
        // Given
        Asset asset = Asset.CreateFolderFromDrive(AssetFixture.NameDefault, AssetFixture.DriveIdDefault);
        asset.Id = AssetFixture.IdDefault;

        // When
        asset.AddOrUpdatePermission(UserFixture.IdDefault, PermissionType.Admin);

        // Then
        Assert.That(
            asset.Permissions,
            Is.EquivalentTo(
                new List<Permission>()
                {
                    new(AssetFixture.IdDefault, UserFixture.IdDefault, PermissionType.Admin),
                }
            ),
            nameof(asset.Permissions)
        );
    }

    [Test]
    public void UpdatesPermissionSuccess()
    {
        // Given
        var asset = AssetFixture.CreateFolderDefaultFromDrive();
        var testPermission = PermissionType.Contributor;

        // When
        asset.AddOrUpdatePermission(UserFixture.IdDefault, testPermission);

        // Then
        Assert.That(
            asset.Permissions,
            Is.EquivalentTo(
                new List<Permission>()
                {
                    new(AssetFixture.IdDefault, UserFixture.IdDefault, testPermission),
                }
            ),
            nameof(asset.Permissions)
        );
    }
}