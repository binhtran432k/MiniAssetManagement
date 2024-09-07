using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Events;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Drives.Delete;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Drives;

public class DeleteDriveHandlerHandle
{
    private readonly IRepository<Drive> _repository = Substitute.For<IRepository<Drive>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private DeleteDriveHandler _handler;

    public DeleteDriveHandlerHandle() => _handler = new DeleteDriveHandler(_repository, _mediator);

    [Test]
    public async Task ReturnsSuccessGivenValidId()
    {
        // Given
        var drive = DriveFixture.CreateDriveDefault();
        _repository
            .FirstOrDefaultAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((Drive?)drive));

        // When
        var result = await _handler.Handle(
            new DeleteDriveCommand(DriveFixture.IdDefault, DriveFixture.OwnerIdDefault),
            CancellationToken.None
        );

        // Then
        await _repository.Received(1).UpdateAsync(drive, Arg.Any<CancellationToken>());
        await _mediator
            .Received(1)
            .Publish(Arg.Any<DriveDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(drive.Status, Is.EqualTo(DriveStatus.Deleted), nameof(drive.Status));
    }

    [Test]
    public async Task ReturnsNotFoundGivenInvalidId()
    {
        // Given
        _repository
            .FirstOrDefaultAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((Drive?)null));

        // When
        var result = await _handler.Handle(
            new DeleteDriveCommand(DriveFixture.IdInvalid, DriveFixture.OwnerIdDefault),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Drive>(), Arg.Any<CancellationToken>());
        await _mediator
            .DidNotReceive()
            .Publish(Arg.Any<DriveDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}