using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders;
using MiniAssetManagement.UseCases.Folders.List;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class ListFoldersFromDriveHandlerHandle
{
    private readonly IListFoldersQueryService _service = Substitute.For<IListFoldersQueryService>();
    private ListFoldersFromDriveHandler _handler;

    public ListFoldersFromDriveHandlerHandle() =>
        _handler = new ListFoldersFromDriveHandler(_service);

    [Test]
    public async Task ReturnsFolders()
    {
        // Given
        List<FolderDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListFromDriveAsync(
                UserFixture.IdDefault,
                FolderFixture.DriveIdDefault,
                Arg.Any<int?>(),
                Arg.Any<int?>()
            )
            .Returns(drives);

        // When
        var result = await _handler.Handle(
            new ListFoldersFromDriveQuery(UserFixture.IdDefault, FolderFixture.DriveIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EquivalentTo(drives), nameof(result.Value));
    }
}