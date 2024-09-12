using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Core.UserAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Users.Update;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Users;

public class UpdateUserHandlerHandle
{
    private readonly IRepository<User> _repository = Substitute.For<IRepository<User>>();
    private UpdateUserHandler _handler;

    public UpdateUserHandlerHandle() => _handler = new UpdateUserHandler(_repository);

    [Test]
    public async Task UpdatesSuccess()
    {
        // Given
        var user = UserFixture.CreateUserDefault();
        _repository
            .FirstOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((User?)user));

        // When
        var result = await _handler.Handle(
            new UpdateUserCommand(UserFixture.IdDefault, UserFixture.UsernameNew),
            CancellationToken.None
        );

        // Then
        await _repository.Received().UpdateAsync(user, Arg.Any<CancellationToken>());
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
                Is.EqualTo(UserFixture.UsernameNew),
                nameof(result.Value.Username)
            );
        });
    }

    [Test]
    public async Task UpdatesFailByNotFound()
    {
        // Given
        _repository
            .FirstOrDefaultAsync(Arg.Any<UserByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((User?)null));

        // When
        var result = await _handler.Handle(
            new UpdateUserCommand(UserFixture.IdInvalid, UserFixture.UsernameDefault),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}