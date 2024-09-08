using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.AddOrUpdatePermission;
using MiniAssetManagement.UseCases.Folders.GetPermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class AddOrUpdatePermissionHandlerHandle
{
    private readonly IGetFolderPermissionQueryService _permissionService =
        Substitute.For<IGetFolderPermissionQueryService>();
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private AddOrUpdatePermissionHandler _handler;

    public AddOrUpdatePermissionHandlerHandle() =>
        _handler = new AddOrUpdatePermissionHandler(_repository, _permissionService);

    [TestCase(nameof(PermissionType.Contributor))]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task AddsOrUpdatesPermissionSuccess(string permissionName)
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        var folder = FolderFixture.CreateFolderDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(
                Arg.Any<FolderWithPermissionsByIdSpec>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(folder);
        var permission = PermissionType.FromName(permissionName);

        // When
        var result = await _handler.Handle(
            new AddOrUpdatePermissionCommand(
                FolderFixture.IdDefault,
                UserFixture.IdDefault,
                UserFixture.IdAlternative,
                permission
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(
            folder.Permissions,
            Is.EquivalentTo(
                new List<Permission>()
                {
                    new(UserFixture.IdDefault, PermissionType.Admin),
                    new(UserFixture.IdAlternative, permission),
                }
            ),
            nameof(folder.Permissions)
        );
    }

    [TestCase(1, 1, nameof(PermissionType.Reader))]
    [TestCase(1, 2, nameof(PermissionType.Admin))]
    public async Task AddsOrUpdatesPermissionFailByConflict(
        int userId,
        int addOrUpdateUserId,
        string permissionName
    )
    {
        // When
        var result = await _handler.Handle(
            new AddOrUpdatePermissionCommand(
                FolderFixture.IdDefault,
                userId,
                addOrUpdateUserId,
                PermissionType.FromName(permissionName)
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Conflict), nameof(result.Status));
    }

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Contributor))]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task AddsOrUpdatesPermissionFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission =
            adminPermissionName == null ? null : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);

        // When
        var result = await _handler.Handle(
            new AddOrUpdatePermissionCommand(
                FolderFixture.IdDefault,
                UserFixture.IdDefault,
                UserFixture.IdAlternative,
                PermissionType.Contributor
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }

    [Test]
    public async Task AddsOrUpdatesPermissionFailByNotFound()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        _repository
            .FirstOrDefaultAsync(Arg.Any<FolderByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((Folder?)null);

        // When
        var result = await _handler.Handle(
            new AddOrUpdatePermissionCommand(
                FolderFixture.IdDefault,
                UserFixture.IdDefault,
                UserFixture.IdAlternative,
                PermissionType.Contributor
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}
