using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Events;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.Delete;
using MiniAssetManagement.UseCases.Folders.Get;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class DeleteFolderHandlerHandle
{
    private readonly IGetFolderQueryService _service = Substitute.For<IGetFolderQueryService>();
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private DeleteFolderHandler _handler;

    public DeleteFolderHandlerHandle() =>
        _handler = new DeleteFolderHandler(_repository, _mediator, _service);

    [Test]
    public async Task ReturnsSuccessGivenValidId()
    {
        // Given
        var folder = FolderFixture.CreateFolderDefaultFromDrive();
        _service.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(folder);

        // When
        var result = await _handler.Handle(
            new DeleteFolderCommand(FolderFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository.Received(1).UpdateAsync(folder, Arg.Any<CancellationToken>());
        await _mediator
            .Received(1)
            .Publish(Arg.Any<FolderDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(folder.Status, Is.EqualTo(FolderStatus.Deleted), nameof(folder.Status));
    }

    [Test]
    public async Task ReturnsNotFoundGivenInvalidId()
    {
        // Given
        _service.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(Task.FromResult((Folder?)null));

        // When
        var result = await _handler.Handle(
            new DeleteFolderCommand(FolderFixture.IdInvalid, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>());
        await _mediator
            .DidNotReceive()
            .Publish(Arg.Any<FolderDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}
