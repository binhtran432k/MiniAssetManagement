using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Infrastructure.Data.Queries;

namespace MiniAssetManagement.IntegrationTests.Data.Queries;

public class TestListDrivesQueryService : BaseTest
{
    private int _testUserId = default;

    [SetUp]
    public async Task SetUpData()
    {
        var userRepository = GetRepository<User>();
        _testUserId = (await userRepository.AddAsync(new("testuser"))).Id;

        var repository = GetRepository<Drive>();
        await repository.AddAsync(new("foobar", _testUserId));
        await repository.AddAsync(new("foo", _testUserId));
        await repository.AddAsync(new("bar", _testUserId));
        await repository.AddAsync(new("baz", _testUserId));
        await repository.AddAsync(new("qux", _testUserId));
    }

    [TestCaseSource(nameof(SourceListAsyncSuccess))]
    public async Task ListAsyncSuccess((int? Skip, int? Take, IEnumerable<string> Expected) props)
    {
        // Given
        ListDrivesQueryService service = new(DbContext);

        // When
        var drives = await service.ListAsync(_testUserId, props.Skip, props.Take);

        // Then
        Assert.That(drives.Select(d => d.Name), Is.EquivalentTo(props.Expected), nameof(drives));
    }

    [Test]
    public async Task ListAsyncNotFound()
    {
        // Given
        ListDrivesQueryService service = new(DbContext);

        // When
        var drives = await service.ListAsync(0);

        // Then
        Assert.That(drives, Is.Empty, nameof(drives));
    }

    public static IEnumerable<(int?, int?, IEnumerable<string>)> SourceListAsyncSuccess()
    {
        yield return (null, null, ["foobar", "foo", "bar", "baz", "qux"]);
        yield return (2, null, ["bar", "baz", "qux"]);
        yield return (null, 1, ["foobar"]);
        yield return (2, 2, ["bar", "baz"]);
    }
}