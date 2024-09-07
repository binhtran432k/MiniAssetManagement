using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Drives.Create;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Drives;

public class CreateDriveHandlerHandle
{
    private readonly IRepository<Drive> _repository = Substitute.For<IRepository<Drive>>();
    private CreateDriveHandler _handler;

    public CreateDriveHandlerHandle() => _handler = new CreateDriveHandler(_repository);

    [Test]
    public async Task ReturnsSuccessGivenValidInput()
    {
        // Given
        var drive = DriveFixture.CreateDriveDefault();
        _repository
            .AddAsync(Arg.Any<Drive>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateDriveCommand(DriveFixture.NameDefault, DriveFixture.OwnerIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(DriveFixture.IdDefault), nameof(result.Value));
    }
}