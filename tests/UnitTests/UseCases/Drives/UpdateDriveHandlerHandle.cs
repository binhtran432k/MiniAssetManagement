using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Drives.Update;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Drives;

public class UpdateDriveHandlerHandle
{
    private readonly IRepository<Drive> _repository = Substitute.For<IRepository<Drive>>();
    private UpdateDriveHandler _handler;

    public UpdateDriveHandlerHandle() => _handler = new UpdateDriveHandler(_repository);

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
            new UpdateDriveCommand(
                DriveFixture.IdDefault,
                DriveFixture.OwnerIdDefault,
                DriveFixture.NameNew
            ),
            CancellationToken.None
        );

        // Then
        await _repository.Received(1).UpdateAsync(drive, Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.Multiple(() =>
        {
            Assert.That(
                result.Value.Id,
                Is.EqualTo(DriveFixture.IdDefault),
                nameof(result.Value.Id)
            );
            Assert.That(
                result.Value.Name,
                Is.EqualTo(DriveFixture.NameNew),
                nameof(result.Value.Name)
            );
        });
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
            new UpdateDriveCommand(
                DriveFixture.IdInvalid,
                DriveFixture.OwnerIdDefault,
                DriveFixture.NameDefault
            ),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Drive>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}