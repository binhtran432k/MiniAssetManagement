using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets.Create;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class CreateAssetFromDriveHandlerHandle
{
    private readonly IRepository<Asset> _repository = Substitute.For<IRepository<Asset>>();
    private readonly IRepository<Drive> _driveRepository = Substitute.For<IRepository<Drive>>();
    private readonly CreateAssetFromDriveHandler _handler;

    public CreateAssetFromDriveHandlerHandle() =>
        _handler = new CreateAssetFromDriveHandler(_repository, _driveRepository);

    [Test]
    public async Task CreatesAssetFromDriveSuccess()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(1);
        var drive = AssetFixture.CreateAssetDefaultFromDrive();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateAssetFromDriveCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.DriveIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(AssetFixture.IdDefault), nameof(result.Value));
    }

    [Test]
    public async Task CreatesAssetFromDriveFailByUnauthorized()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(0);
        var drive = AssetFixture.CreateAssetDefaultFromDrive();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateAssetFromDriveCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.DriveIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }
}
