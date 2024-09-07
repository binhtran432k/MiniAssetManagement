using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Core.UserAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Users.Get;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Users;

public class GetUserHandlerHandle
{
    private readonly IReadRepository<User> _repository = Substitute.For<IReadRepository<User>>();
    private GetUserHandler _handler;

    public GetUserHandlerHandle() => _handler = new GetUserHandler(_repository);

    [Test]
    public async Task ReturnsSuccessGivenValidId()
    {
        // Given
        var user = UserFixture.CreateUserDefault();
        _repository
            .FirstOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((User?)user));

        // When
        var result = await _handler.Handle(
            new GetUserQuery(UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.Multiple(() =>
        {
            Assert.That(
                result.Value.Id,
                Is.EqualTo(UserFixture.IdDefault),
                nameof(result.Value.Id)
            );
            Assert.That(
                result.Value.Username,
                Is.EqualTo(UserFixture.UsernameDefault),
                nameof(result.Value.Username)
            );
        });
    }

    [Test]
    public async Task ReturnsNotFoundGivenInvalidId()
    {
        // Given
        _repository
            .FirstOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((User?)null));

        // When
        var result = await _handler.Handle(
            new GetUserQuery(UserFixture.IdInvalid),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}