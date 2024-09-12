using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Infrastructure.Data.Queries;

namespace MiniAssetManagement.IntegrationTests.Data.Queries;

public class TestListAssetsQueryService : BaseTest
{
    private int _testUserId;
    private int _testDriveId;
    private int _testAssetId;
    private int _testFromDriveCount;
    private int _testFromAssetCount;

    [SetUp]
    public async Task SetUpData()
    {
        var userRepository = GetRepository<User>();
        _testUserId = (await userRepository.AddAsync(new("testuser"))).Id;

        var driveRepository = GetRepository<Drive>();
        _testDriveId = (await driveRepository.AddAsync(new("testdrive", _testUserId))).Id;

        var repository = GetRepository<Asset>();
        _testAssetId = (
            await repository.AddAsync(Asset.CreateFromDrive("foobar", _testDriveId))
        ).Id;

        await repository.AddAsync(Asset.CreateFromDrive("foo", _testDriveId));
        await repository.AddAsync(Asset.CreateFromDrive("bar", _testDriveId));
        await repository.AddAsync(Asset.CreateFromDrive("baz", _testDriveId));
        await repository.AddAsync(Asset.CreateFromDrive("qux", _testDriveId));
        _testFromDriveCount = 5;

        var deletedFromDrive = Asset.CreateFromDrive("deletedasset", _testDriveId);
        deletedFromDrive.UpdateStatus(AssetStatus.Deleted);
        await repository.AddAsync(deletedFromDrive);

        await repository.AddAsync(Asset.CreateFromAsset("foobar2", _testAssetId));
        await repository.AddAsync(Asset.CreateFromAsset("foo2", _testAssetId));
        await repository.AddAsync(Asset.CreateFromAsset("bar2", _testAssetId));
        await repository.AddAsync(Asset.CreateFromAsset("baz2", _testAssetId));
        await repository.AddAsync(Asset.CreateFromAsset("qux2", _testAssetId));
        _testFromAssetCount = 5;

        var deletedFromAsset = Asset.CreateFromAsset("deletedasset2", _testAssetId);
        deletedFromAsset.UpdateStatus(AssetStatus.Deleted);
        await repository.AddAsync(deletedFromAsset);

        await repository.SaveChangesAsync();
    }

    [TestCaseSource(nameof(SourceListFromDriveAsyncSuccess))]
    public async Task ListFromDriveAsyncSuccess(
        (int? Skip, int? Take, IEnumerable<string> Expected) props
    )
    {
        // Given
        ListAssetsQueryService service = new(DbContext);

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

    [TestCaseSource(nameof(SourceListFromAssetAsyncSuccess))]
    public async Task ListFromAssetAsyncSuccess(
        (int? Skip, int? Take, IEnumerable<string> Expected) props
    )
    {
        // Given
        ListAssetsQueryService service = new(DbContext);

        // When
        var (drives, count) = await service.ListFromAssetAsync(
            _testAssetId,
            props.Skip,
            props.Take
        );

        // Then
        Assert.That(drives.Select(d => d.Name), Is.EquivalentTo(props.Expected), nameof(drives));
        Assert.That(count, Is.EqualTo(_testFromAssetCount), nameof(count));
    }

    [Test]
    public async Task ListFromDriveAsyncNotFound()
    {
        // Given
        ListAssetsQueryService service = new(DbContext);

        // When
        var (drives, count) = await service.ListFromDriveAsync(0);

        // Then
        Assert.That(drives, Is.Empty, nameof(drives));
        Assert.That(count, Is.EqualTo(0), nameof(count));
    }

    [Test]
    public async Task ListFromAssetAsyncNotFound()
    {
        // Given
        ListAssetsQueryService service = new(DbContext);

        // When
        var (drives, count) = await service.ListFromAssetAsync(0);

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

    public static IEnumerable<(int?, int?, IEnumerable<string>)> SourceListFromAssetAsyncSuccess()
    {
        yield return (null, null, ["foobar2", "foo2", "bar2", "baz2", "qux2"]);
        yield return (2, null, ["bar2", "baz2", "qux2"]);
        yield return (null, 1, ["foobar2"]);
        yield return (2, 2, ["bar2", "baz2"]);
    }
}
