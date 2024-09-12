using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets.Create;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class CreateAssetFromDriveHandlerHandle
{
    private readonly IRepository<Asset> _repository = Substitute.For<IRepository<Asset>>();
    private readonly IRepository<Drive> _driveRepository = Substitute.For<IRepository<Drive>>();
    private readonly CreateFolderFromDriveHandler _folderHandler;
    private readonly CreateFileFromDriveHandler _fileHandler;

    public CreateAssetFromDriveHandlerHandle() =>
        (_folderHandler, _fileHandler) = (
            new CreateFolderFromDriveHandler(_repository, _driveRepository),
            new CreateFileFromDriveHandler(_repository, _driveRepository)
        );

    [Test]
    public async Task CreatesFolderFromDriveSuccess()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(1);
        var asset = AssetFixture.CreateFolderDefaultFromDrive();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _folderHandler.Handle(
            new CreateFolderFromDriveCommand(
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
    public async Task CreatesFileFromDriveSuccess()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(1);
        var asset = AssetFixture.CreateFileDefaultFromDrive();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _fileHandler.Handle(
            new CreateFileFromDriveCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.DriveIdDefault,
                AssetFixture.FileTypeDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(AssetFixture.IdDefault), nameof(result.Value));
    }

    [Test]
    public async Task CreatesFolderFromDriveFailByUnauthorized()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(0);
        var asset = AssetFixture.CreateFolderDefaultFromDrive();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _folderHandler.Handle(
            new CreateFolderFromDriveCommand(
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

    [Test]
    public async Task CreatesFileFromDriveFailByUnauthorized()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(0);
        var asset = AssetFixture.CreateFileDefaultFromDrive();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _fileHandler.Handle(
            new CreateFileFromDriveCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.DriveIdDefault,
                AssetFixture.FileTypeDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }
}
