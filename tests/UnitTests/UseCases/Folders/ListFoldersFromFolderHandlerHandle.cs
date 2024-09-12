using Ardalis.Result;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders;
using MiniAssetManagement.UseCases.Folders.GetPermission;
using MiniAssetManagement.UseCases.Folders.List;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class ListFoldersFromFolderHandlerHandle
{
    private readonly IListFoldersQueryService _service = Substitute.For<IListFoldersQueryService>();
    private readonly IGetFolderPermissionQueryService _permissionService =
        Substitute.For<IGetFolderPermissionQueryService>();
    private ListFoldersFromFolderHandler _handler;

    public ListFoldersFromFolderHandlerHandle() =>
        _handler = new ListFoldersFromFolderHandler(_service, _permissionService);

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task ListsFoldersFromFolderSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        List<FolderDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListFromFolderAsync(FolderFixture.ParentIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns((drives, 2));

        // When
        var result = await _handler.Handle(
            new ListFoldersFromFolderQuery(UserFixture.IdDefault, FolderFixture.ParentIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value.Item1, Is.EquivalentTo(drives), nameof(result.Value.Item1));
        Assert.That(result.Value.Item2, Is.EqualTo(2), nameof(result.Value.Item2));
    }

    [Test]
    public async Task ListsFoldersFromFolderFailByUnauthorized()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns((PermissionType?)null);
        List<FolderDTO> drives = [new(1, "foo"), new(2, "bar")];
        _service
            .ListFromFolderAsync(FolderFixture.ParentIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns((drives, 2));

        // When
        var result = await _handler.Handle(
            new ListFoldersFromFolderQuery(UserFixture.IdDefault, FolderFixture.ParentIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }
}
