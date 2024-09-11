using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.Create;
using MiniAssetManagement.UseCases.Folders.GetPermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class CreateFolderFromFolderHandlerHandle
{
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private readonly IGetFolderPermissionQueryService _permissionService =
        Substitute.For<IGetFolderPermissionQueryService>();
    private readonly CreateFolderFromFolderHandler _handler;

    public CreateFolderFromFolderHandlerHandle() =>
        _handler = new CreateFolderFromFolderHandler(_repository, _permissionService);

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    public async Task CreatesFolderFromFolderSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        var drive = FolderFixture.CreateFolderDefaultFromFolder();
        _repository
            .AddAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateFolderFromFolderCommand(
                FolderFixture.NameDefault,
                UserFixture.IdDefault,
                FolderFixture.ParentIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(FolderFixture.IdDefault), nameof(result.Value));
    }

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task CreatesFolderFromFolderFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission =
            adminPermissionName is null ? null : PermissionType.FromName(adminPermissionName);
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(adminPermission);
        var drive = FolderFixture.CreateFolderDefaultFromFolder();
        _repository
            .AddAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateFolderFromFolderCommand(
                FolderFixture.NameDefault,
                UserFixture.IdDefault,
                FolderFixture.ParentIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }
}