using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders;
using MiniAssetManagement.UseCases.Folders.List;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class ListFoldersFromDriveHandlerHandle
{
    private readonly IListFoldersQueryService _service = Substitute.For<IListFoldersQueryService>();
    private readonly IRepository<Drive> _driveRepository = Substitute.For<IRepository<Drive>>();
    private ListFoldersFromDriveHandler _handler;

    public ListFoldersFromDriveHandlerHandle() =>
        _handler = new ListFoldersFromDriveHandler(_driveRepository, _service);

    [Test]
    public async Task ListsFoldersSuccess()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(1);
        List<FolderDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListFromDriveAsync(FolderFixture.DriveIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns((drives, 2));

        // When
        var result = await _handler.Handle(
            new ListFoldersFromDriveQuery(UserFixture.IdDefault, FolderFixture.DriveIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value.Item1, Is.EquivalentTo(drives), nameof(result.Value.Item1));
        Assert.That(result.Value.Item2, Is.EqualTo(2), nameof(result.Value.Item2));
    }

    [Test]
    public async Task ListsFoldersFailByUnauthorized()
    {
        // Given
        _driveRepository
            .CountAsync(Arg.Any<DriveByIdAndOwnerIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(0);
        List<FolderDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListFromDriveAsync(FolderFixture.DriveIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns((drives, 2));

        // When
        var result = await _handler.Handle(
            new ListFoldersFromDriveQuery(UserFixture.IdDefault, FolderFixture.DriveIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }
}
