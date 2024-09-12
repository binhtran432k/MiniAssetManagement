using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets.GetPermission;
using MiniAssetManagement.UseCases.Assets.RemovePermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class RemoveAssetPermissionHandlerHandle
{
    private readonly IGetAssetPermissionQueryService _permissionService =
        Substitute.For<IGetAssetPermissionQueryService>();
    private readonly IRepository<Asset> _repository = Substitute.For<IRepository<Asset>>();
    private RemoveAssetPermissionHandler _handler;

    public RemoveAssetPermissionHandlerHandle() =>
        _handler = new RemoveAssetPermissionHandler(_repository, _permissionService);

    [Test]
    public async Task RemovesPermissionSuccess()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        var asset = AssetFixture.CreateFolderDefaultFromDrive();
        asset.AddOrUpdatePermission(UserFixture.IdAlternative, PermissionType.Reader);
        _repository
            .FirstOrDefaultAsync(
                Arg.Any<AssetWithPermissionsByIdSpec>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(asset);

        // When
        var result = await _handler.Handle(
            new RemoveAssetPermissionCommand(
                AssetFixture.IdDefault,
                UserFixture.IdDefault,
                UserFixture.IdAlternative
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(
            asset.Permissions,
            Is.EquivalentTo(
                new List<Permission>()
                {
                    new(AssetFixture.IdDefault, UserFixture.IdDefault, PermissionType.Admin),
                }
            ),
            nameof(asset.Permissions)
        );
    }

    [TestCase(1, 1)]
    public async Task RemovesPermissionFailByConflict(int userId, int removeUserId)
    {
        // When
        var result = await _handler.Handle(
            new RemoveAssetPermissionCommand(AssetFixture.IdDefault, userId, removeUserId),
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
        PermissionType? adminPermission = adminPermissionName is null
            ? null
            : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);

        // When
        var result = await _handler.Handle(
            new RemoveAssetPermissionCommand(
                AssetFixture.IdDefault,
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
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((Asset?)null);

        // When
        var result = await _handler.Handle(
            new RemoveAssetPermissionCommand(
                AssetFixture.IdDefault,
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