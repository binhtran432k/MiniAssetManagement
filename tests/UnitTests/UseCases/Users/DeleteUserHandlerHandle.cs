using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.UserAggregate;
using MiniAssetManagement.Core.UserAggregate.Events;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Users.Delete;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Users;

public class DeleteUserHandlerHandle
{
    private readonly IRepository<User> _repository = Substitute.For<IRepository<User>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private DeleteUserHandler _handler;

    public DeleteUserHandlerHandle() => _handler = new DeleteUserHandler(_repository, _mediator);

    [Test]
    public async Task ReturnsSuccessGivenValidId()
    {
        // Mock
        var user = UserFixture.GetUserDefault();
        _repository
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((User?)user));

        // When
        var result = await _handler.Handle(
            new DeleteUserCommand(UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository.Received(1).UpdateAsync(user, Arg.Any<CancellationToken>());
        await _mediator
            .Received(1)
            .Publish(Arg.Any<UserDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
    }

    [Test]
    public async Task ReturnsNotFoundGivenInvalidId()
    {
        // Mock
        _repository
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((User?)null));

        // When
        var result = await _handler.Handle(
            new DeleteUserCommand(UserFixture.IdInvalid),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
        await _mediator
            .DidNotReceive()
            .Publish(Arg.Any<UserDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}