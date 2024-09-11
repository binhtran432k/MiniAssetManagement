using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Infrastructure.Data.Queries;

namespace MiniAssetManagement.IntegrationTests.Data.Queries;

public class TestGetFolderPermissionQueryService : BaseTest
{
    private int _testUserId1 = default;
    private int _testUserId2 = default;
    private int _testDriveId = default;
    private int _testFolderId = default;
    private int _testSubFolderId = default;
    private readonly PermissionType _testPermission = PermissionType.Admin;

    [SetUp]
    public async Task SetUpData()
    {
        var userRepository = GetRepository<User>();
        _testUserId1 = (await userRepository.AddAsync(new("testuser"))).Id;
        _testUserId2 = (await userRepository.AddAsync(new("altuser"))).Id;

        var driveRepository = GetRepository<Drive>();
        _testDriveId = (await driveRepository.AddAsync(new("testdrive", _testUserId1))).Id;

        var folderRepository = GetRepository<Folder>();
        var folder = Folder.CreateFromDrive("testfolder", _testDriveId);
        folder.AddOrUpdatePermission(_testUserId1, _testPermission);
        _testFolderId = (await folderRepository.AddAsync(folder)).Id;

        _testSubFolderId = (
            await folderRepository.AddAsync(Folder.CreateFromFolder("subfolder", _testFolderId))
        ).Id;
    }

    [Test]
    public async Task GetAsyncSuccess()
    {
        // Given
        GetFolderPermissionQueryService service = new(DbContext);

        // When
        var permission = await service.GetAsync(_testFolderId, _testUserId1);

        // Then
        Assert.That(permission, Is.EqualTo(_testPermission), nameof(permission));
    }

    [Test]
    public async Task GetAsyncSuccessInSubFolder()
    {
        // Given
        GetFolderPermissionQueryService service = new(DbContext);
        Assert.That(_testSubFolderId, Is.Not.EqualTo(_testFolderId), nameof(_testSubFolderId));

        // When
        var permission = await service.GetAsync(_testSubFolderId, _testUserId1);

        // Then
        Assert.That(permission, Is.EqualTo(_testPermission), nameof(permission));
    }

    [Test]
    public async Task GetAsyncNotFound()
    {
        // Given
        GetFolderPermissionQueryService service = new(DbContext);

        // When
        var permission = await service.GetAsync(_testFolderId, _testUserId2);

        // Then
        Assert.That(permission, Is.Null, nameof(permission));
    }
}
