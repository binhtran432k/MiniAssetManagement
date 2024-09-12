using Ardalis.SharedKernel;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Users.Create;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Users;

public class CreateUserHandlerHandle
{
    private readonly IRepository<User> _repository = Substitute.For<IRepository<User>>();
    private CreateUserHandler _handler;

    public CreateUserHandlerHandle() => _handler = new CreateUserHandler(_repository);

    [Test]
    public async Task CreatesSuccess()
    {
        // Given
        var user = UserFixture.CreateUserDefault();
        _repository
            .AddAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(user));

        // When
        var result = await _handler.Handle(
            new CreateUserCommand(UserFixture.UsernameDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(UserFixture.IdDefault), nameof(result.Value));
    }
}