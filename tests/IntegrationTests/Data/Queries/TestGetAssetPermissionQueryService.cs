using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Infrastructure.Data.Queries;

namespace MiniAssetManagement.IntegrationTests.Data.Queries;

public class TestGetAssetPermissionQueryService : BaseTest
{
    private int _testUserId1 = default;
    private int _testUserId2 = default;
    private int _testDriveId = default;
    private int _testAssetId = default;
    private int _testSubAssetId = default;
    private readonly PermissionType _testPermission = PermissionType.Admin;

    [SetUp]
    public async Task SetUpData()
    {
        var userRepository = GetRepository<User>();
        _testUserId1 = (await userRepository.AddAsync(new("testuser"))).Id;
        _testUserId2 = (await userRepository.AddAsync(new("altuser"))).Id;

        var driveRepository = GetRepository<Drive>();
        _testDriveId = (await driveRepository.AddAsync(new("testdrive", _testUserId1))).Id;

        var assetRepository = GetRepository<Asset>();
        var asset = Asset.CreateFromDrive("testasset", _testDriveId);
        asset.AddOrUpdatePermission(_testUserId1, _testPermission);
        _testAssetId = (await assetRepository.AddAsync(asset)).Id;

        _testSubAssetId = (
            await assetRepository.AddAsync(Asset.CreateFromAsset("subasset", _testAssetId))
        ).Id;
    }

    [Test]
    public async Task GetAsyncSuccess()
    {
        // Given
        GetAssetPermissionQueryService service = new(DbContext);

        // When
        var permission = await service.GetAsync(_testAssetId, _testUserId1);

        // Then
        Assert.That(permission, Is.EqualTo(_testPermission), nameof(permission));
    }

    [Test]
    public async Task GetAsyncSuccessInSubAsset()
    {
        // Given
        GetAssetPermissionQueryService service = new(DbContext);
        Assert.That(_testSubAssetId, Is.Not.EqualTo(_testAssetId), nameof(_testSubAssetId));

        // When
        var permission = await service.GetAsync(_testSubAssetId, _testUserId1);

        // Then
        Assert.That(permission, Is.EqualTo(_testPermission), nameof(permission));
    }

    [Test]
    public async Task GetAsyncNotFound()
    {
        // Given
        GetAssetPermissionQueryService service = new(DbContext);

        // When
        var permission = await service.GetAsync(_testAssetId, _testUserId2);

        // Then
        Assert.That(permission, Is.Null, nameof(permission));
    }
}
