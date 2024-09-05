using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Users.List;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Users;

public class ListUsersHandlerHandle
{
    private readonly IListUsersQueryService _service = Substitute.For<IListUsersQueryService>();
    private ListUsersHandler _handler;

    public ListUsersHandlerHandle() => _handler = new ListUsersHandler(_service);

    [Test]
    public async Task ReturnsEmptyWhenEmpty()
    {
        // Mock
        _service.ListAsync().Returns([]);

        // When
        var result = await _handler.Handle(new ListUsersQuery(null, null), CancellationToken.None);

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.Empty, nameof(result.Value));
    }

    [Test]
    public async Task ReturnsUsersWhenNotEmpty()
    {
        // Mock
        var users = UserFixture.GetListUserDTODefault();
        _service.ListAsync().Returns(users);

        // When
        var result = await _handler.Handle(new ListUsersQuery(null, null), CancellationToken.None);

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EquivalentTo(users), nameof(result.Value));
    }
}
