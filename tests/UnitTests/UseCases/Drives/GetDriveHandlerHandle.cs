using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Drives.Get;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Drives;

public class GetDriveHandlerHandle
{
    private readonly IReadRepository<Drive> _repository = Substitute.For<IReadRepository<Drive>>();
    private GetDriveHandler _handler;

    public GetDriveHandlerHandle() => _handler = new GetDriveHandler(_repository);

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
            new GetDriveQuery(DriveFixture.IdDefault, DriveFixture.OwnerIdDefault),
            CancellationToken.None
        );

        // Then
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
                Is.EqualTo(DriveFixture.NameDefault),
                nameof(result.Value.Name)
            );
        });
    }

    [Test]
    public async Task ReturnsNotFoundGivenInvalidId()
    {
        // Given
        _repository
            .FirstOrDefaultAsync(Arg.Any<DrivesByOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult((Drive?)null));

        // When
        var result = await _handler.Handle(
            new GetDriveQuery(DriveFixture.IdDefault, DriveFixture.OwnerIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}
