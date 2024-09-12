using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Infrastructure.Data.Queries;

namespace MiniAssetManagement.IntegrationTests.Data.Queries;

public class TestListFoldersQueryService : BaseTest
{
    private int _testUserId;
    private int _testDriveId;
    private int _testFolderId;
    private int _testFromDriveCount;
    private int _testFromFolderCount;

    [SetUp]
    public async Task SetUpData()
    {
        var userRepository = GetRepository<User>();
        _testUserId = (await userRepository.AddAsync(new("testuser"))).Id;

        var driveRepository = GetRepository<Drive>();
        _testDriveId = (await driveRepository.AddAsync(new("testdrive", _testUserId))).Id;

        var repository = GetRepository<Folder>();
        _testFolderId = (
            await repository.AddAsync(Folder.CreateFromDrive("foobar", _testDriveId))
        ).Id;

        await repository.AddAsync(Folder.CreateFromDrive("foo", _testDriveId));
        await repository.AddAsync(Folder.CreateFromDrive("bar", _testDriveId));
        await repository.AddAsync(Folder.CreateFromDrive("baz", _testDriveId));
        await repository.AddAsync(Folder.CreateFromDrive("qux", _testDriveId));
        _testFromDriveCount = 5;

        var deletedFromDrive = Folder.CreateFromDrive("deletedfolder", _testDriveId);
        deletedFromDrive.UpdateStatus(FolderStatus.Deleted);
        await repository.AddAsync(deletedFromDrive);

        await repository.AddAsync(Folder.CreateFromFolder("foobar2", _testFolderId));
        await repository.AddAsync(Folder.CreateFromFolder("foo2", _testFolderId));
        await repository.AddAsync(Folder.CreateFromFolder("bar2", _testFolderId));
        await repository.AddAsync(Folder.CreateFromFolder("baz2", _testFolderId));
        await repository.AddAsync(Folder.CreateFromFolder("qux2", _testFolderId));
        _testFromFolderCount = 5;

        var deletedFromFolder = Folder.CreateFromFolder("deletedfolder2", _testFolderId);
        deletedFromFolder.UpdateStatus(FolderStatus.Deleted);
        await repository.AddAsync(deletedFromFolder);

        await repository.SaveChangesAsync();
    }

    [TestCaseSource(nameof(SourceListFromDriveAsyncSuccess))]
    public async Task ListFromDriveAsyncSuccess(
        (int? Skip, int? Take, IEnumerable<string> Expected) props
    )
    {
        // Given
        ListFoldersQueryService service = new(DbContext);

        // When
        var (drives, count) = await service.ListFromDriveAsync(
            _testDriveId,
            props.Skip,
            props.Take
        );

        // Then
        Assert.That(drives.Select(d => d.Name), Is.EquivalentTo(props.Expected), nameof(drives));
        Assert.That(count, Is.EqualTo(_testFromDriveCount), nameof(count));
    }

    [TestCaseSource(nameof(SourceListFromFolderAsyncSuccess))]
    public async Task ListFromFolderAsyncSuccess(
        (int? Skip, int? Take, IEnumerable<string> Expected) props
    )
    {
        // Given
        ListFoldersQueryService service = new(DbContext);

        // When
        var (drives, count) = await service.ListFromFolderAsync(
            _testFolderId,
            props.Skip,
            props.Take
        );

        // Then
        Assert.That(drives.Select(d => d.Name), Is.EquivalentTo(props.Expected), nameof(drives));
        Assert.That(count, Is.EqualTo(_testFromFolderCount), nameof(count));
    }

    [Test]
    public async Task ListFromDriveAsyncNotFound()
    {
        // Given
        ListFoldersQueryService service = new(DbContext);

        // When
        var (drives, count) = await service.ListFromDriveAsync(0);

        // Then
        Assert.That(drives, Is.Empty, nameof(drives));
        Assert.That(count, Is.EqualTo(0), nameof(count));
    }

    [Test]
    public async Task ListFromFolderAsyncNotFound()
    {
        // Given
        ListFoldersQueryService service = new(DbContext);

        // When
        var (drives, count) = await service.ListFromFolderAsync(0);

        // Then
        Assert.That(drives, Is.Empty, nameof(drives));
        Assert.That(count, Is.EqualTo(0), nameof(count));
    }

    public static IEnumerable<(int?, int?, IEnumerable<string>)> SourceListFromDriveAsyncSuccess()
    {
        yield return (null, null, ["foobar", "foo", "bar", "baz", "qux"]);
        yield return (2, null, ["bar", "baz", "qux"]);
        yield return (null, 1, ["foobar"]);
        yield return (2, 2, ["bar", "baz"]);
    }

    public static IEnumerable<(int?, int?, IEnumerable<string>)> SourceListFromFolderAsyncSuccess()
    {
        yield return (null, null, ["foobar2", "foo2", "bar2", "baz2", "qux2"]);
        yield return (2, null, ["bar2", "baz2", "qux2"]);
        yield return (null, 1, ["foobar2"]);
        yield return (2, 2, ["bar2", "baz2"]);
    }
}
