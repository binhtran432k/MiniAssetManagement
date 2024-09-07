using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders;
using MiniAssetManagement.UseCases.Folders.List;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class ListFoldersFromFolderHandlerHandle
{
    private readonly IListFoldersQueryService _service = Substitute.For<IListFoldersQueryService>();
    private ListFoldersFromFolderHandler _handler;

    public ListFoldersFromFolderHandlerHandle() =>
        _handler = new ListFoldersFromFolderHandler(_service);

    [Test]
    public async Task ReturnsFolders()
    {
        // Given
        List<FolderDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListFromFolderAsync(
                UserFixture.IdDefault,
                FolderFixture.ParentIdDefault,
                Arg.Any<int?>(),
                Arg.Any<int?>()
            )
            .Returns(drives);

        // When
        var result = await _handler.Handle(
            new ListFoldersFromFolderQuery(UserFixture.IdDefault, FolderFixture.ParentIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EquivalentTo(drives), nameof(result.Value));
    }
}