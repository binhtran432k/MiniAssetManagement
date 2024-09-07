using MiniAssetManagement.UseCases.Users;
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
        // Given
        _service.ListAsync().Returns([]);

        // When
        var result = await _handler.Handle(new ListUsersQuery(), CancellationToken.None);

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.Empty, nameof(result.Value));
    }

    [Test]
    public async Task ReturnsUsersWhenNotEmpty()
    {
        // Given
        List<UserDTO> users = [new(1, "foo"), new(2, "bar")];
        _service.ListAsync().Returns(users);

        // When
        var result = await _handler.Handle(new ListUsersQuery(), CancellationToken.None);

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EquivalentTo(users), nameof(result.Value));
    }
}
