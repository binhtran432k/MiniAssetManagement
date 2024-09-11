using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.GetPermission;
using MiniAssetManagement.UseCases.Folders.Update;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class UpdateFolderHandlerHandle
{
    private readonly IGetFolderPermissionQueryService _permissionService =
        Substitute.For<IGetFolderPermissionQueryService>();
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private UpdateFolderHandler _handler;

    public UpdateFolderHandlerHandle() =>
        _handler = new UpdateFolderHandler(_repository, _permissionService);

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    public async Task UpdatesFolderSuccess(string adminPermissionName)
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
            new UpdateFolderCommand(
                FolderFixture.IdDefault,
                UserFixture.IdDefault,
                FolderFixture.NameNew
            ),
            CancellationToken.None
        );

        // Then
        await _repository.Received(1).UpdateAsync(folder, Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.Multiple(() =>
        {
            Assert.That(
                result.Value.Id,
                Is.EqualTo(FolderFixture.IdDefault),
                nameof(result.Value.Id)
            );
            Assert.That(
                result.Value.Name,
                Is.EqualTo(FolderFixture.NameNew),
                nameof(result.Value.Name)
            );
        });
    }

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task UpdatesFolderFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission =
            adminPermissionName is null ? null : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);

        // When
        var result = await _handler.Handle(
            new UpdateFolderCommand(
                FolderFixture.IdInvalid,
                UserFixture.IdDefault,
                FolderFixture.NameDefault
            ),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>());
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
            new UpdateFolderCommand(
                FolderFixture.IdInvalid,
                UserFixture.IdDefault,
                FolderFixture.NameDefault
            ),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}
