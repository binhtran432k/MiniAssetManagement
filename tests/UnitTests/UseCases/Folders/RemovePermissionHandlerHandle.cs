using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.GetPermission;
using MiniAssetManagement.UseCases.Folders.RemovePermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class RemovePermissionHandlerHandle
{
    private readonly IGetFolderPermissionQueryService _permissionService =
        Substitute.For<IGetFolderPermissionQueryService>();
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private RemovePermissionHandler _handler;

    public RemovePermissionHandlerHandle() =>
        _handler = new RemovePermissionHandler(_repository, _permissionService);

    [Test]
    public async Task RemovesPermissionSuccess()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        var folder = FolderFixture.CreateFolderDefaultFromDrive();
        folder.AddOrUpdatePermission(UserFixture.IdAlternative, PermissionType.Reader);
        _repository
            .FirstOrDefaultAsync(
                Arg.Any<FolderWithPermissionsByIdSpec>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(folder);

        // When
        var result = await _handler.Handle(
            new RemovePermissionCommand(
                FolderFixture.IdDefault,
                UserFixture.IdDefault,
                UserFixture.IdAlternative
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(
            folder.Permissions,
            Is.EquivalentTo(
                new List<Permission>() { new(UserFixture.IdDefault, PermissionType.Admin) }
            ),
            nameof(folder.Permissions)
        );
    }

    [TestCase(1, 1)]
    public async Task RemovesPermissionFailByConflict(int userId, int removeUserId)
    {
        // When
        var result = await _handler.Handle(
            new RemovePermissionCommand(FolderFixture.IdDefault, userId, removeUserId),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Conflict), nameof(result.Status));
    }

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Contributor))]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task RemovesPermissionFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission =
            adminPermissionName == null ? null : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);

        // When
        var result = await _handler.Handle(
            new RemovePermissionCommand(
                FolderFixture.IdDefault,
                UserFixture.IdDefault,
                UserFixture.IdAlternative
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }

    [Test]
    public async Task RemovesPermissionFailByNotFound()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        _repository
            .FirstOrDefaultAsync(Arg.Any<FolderByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((Folder?)null);

        // When
        var result = await _handler.Handle(
            new RemovePermissionCommand(
                FolderFixture.IdDefault,
                UserFixture.IdDefault,
                UserFixture.IdAlternative
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}
