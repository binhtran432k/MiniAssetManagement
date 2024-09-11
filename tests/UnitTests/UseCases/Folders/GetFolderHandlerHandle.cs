using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.Get;
using MiniAssetManagement.UseCases.Folders.GetPermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class GetFolderHandlerHandle
{
    private readonly IGetFolderPermissionQueryService _permissionService =
        Substitute.For<IGetFolderPermissionQueryService>();
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private GetFolderHandler _handler;

    public GetFolderHandlerHandle() =>
        _handler = new GetFolderHandler(_repository, _permissionService);

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task GetsFolderSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        var folder = FolderFixture.CreateFolderDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(Arg.Any<FolderByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(folder);

        // When
        var result = await _handler.Handle(
            new GetFolderQuery(FolderFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.Multiple(() =>
        {
            Assert.That(result.Value.Id, Is.EqualTo(folder.Id), nameof(result.Value.Id));
            Assert.That(result.Value.Name, Is.EqualTo(folder.Name), nameof(result.Value.Name));
        });
    }

    [Test]
    public async Task GetsFolderFailByUnauthorized()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns((PermissionType?)null);

        // When
        var result = await _handler.Handle(
            new GetFolderQuery(FolderFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }

    [Test]
    public async Task GetsFolderFailByNotFound()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        _repository
            .FirstOrDefaultAsync(Arg.Any<FolderByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((Folder?)null);

        // When
        var result = await _handler.Handle(
            new GetFolderQuery(FolderFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}
