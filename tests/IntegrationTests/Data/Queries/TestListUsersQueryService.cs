using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Infrastructure.Data.Queries;

namespace MiniAssetManagement.IntegrationTests.Data.Queries;

public class TestListUsersQueryService : BaseTest
{
    [SetUp]
    public async Task SetUpUser()
    {
        var repository = GetRepository<User>();
        await repository.AddAsync(new("foobar"));
        await repository.AddAsync(new("foo"));
        await repository.AddAsync(new("bar"));
        await repository.AddAsync(new("baz"));
        await repository.AddAsync(new("qux"));
    }

    [TestCaseSource(nameof(SourceListAsyncSuccess))]
    public async Task ListAsyncSuccess((int? Skip, int? Take, IEnumerable<string> Expected) props)
    {
        // Given
        ListUsersQueryService service = new(DbContext);

        // When
        var users = await service.ListAsync(props.Skip, props.Take);

        // Then
        Assert.That(users.Select(u => u.Username), Is.EquivalentTo(props.Expected), nameof(users));
    }

    public static IEnumerable<(int?, int?, IEnumerable<string>)> SourceListAsyncSuccess()
    {
        yield return (null, null, ["foobar", "foo", "bar", "baz", "qux"]);
        yield return (2, null, ["bar", "baz", "qux"]);
        yield return (null, 1, ["foobar"]);
        yield return (2, 2, ["bar", "baz"]);
    }
}