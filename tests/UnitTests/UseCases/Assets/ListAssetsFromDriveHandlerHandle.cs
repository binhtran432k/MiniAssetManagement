using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets;
using MiniAssetManagement.UseCases.Assets.List;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class ListAssetsFromDriveHandlerHandle
{
    private readonly IListAssetsQueryService _service = Substitute.For<IListAssetsQueryService>();
    private readonly IRepository<Drive> _driveRepository = Substitute.For<IRepository<Drive>>();
    private ListAssetsFromDriveHandler _handler;

    public ListAssetsFromDriveHandlerHandle() =>
        _handler = new ListAssetsFromDriveHandler(_driveRepository, _service);

    [Test]
    public async Task ListsAssetsSuccess()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(1);
        List<AssetDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListFromDriveAsync(AssetFixture.DriveIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns((drives, 2));

        // When
        var result = await _handler.Handle(
            new ListAssetsFromDriveQuery(UserFixture.IdDefault, AssetFixture.DriveIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value.Item1, Is.EquivalentTo(drives), nameof(result.Value.Item1));
        Assert.That(result.Value.Item2, Is.EqualTo(2), nameof(result.Value.Item2));
    }

    [Test]
    public async Task ListsAssetsFailByUnauthorized()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(0);
        List<AssetDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListFromDriveAsync(AssetFixture.DriveIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns((drives, 2));

        // When
        var result = await _handler.Handle(
            new ListAssetsFromDriveQuery(UserFixture.IdDefault, AssetFixture.DriveIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }
}
