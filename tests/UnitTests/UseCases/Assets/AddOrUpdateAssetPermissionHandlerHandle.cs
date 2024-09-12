using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets.AddOrUpdatePermission;
using MiniAssetManagement.UseCases.Assets.GetPermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class AddOrUpdateAssetPermissionHandlerHandle
{
    private readonly IGetAssetPermissionQueryService _permissionService =
        Substitute.For<IGetAssetPermissionQueryService>();
    private readonly IRepository<Asset> _repository = Substitute.For<IRepository<Asset>>();
    private AddOrUpdateAssetPermissionHandler _handler;

    public AddOrUpdateAssetPermissionHandlerHandle() =>
        _handler = new AddOrUpdateAssetPermissionHandler(_repository, _permissionService);

    [TestCase(nameof(PermissionType.Contributor))]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task AddsOrUpdatesPermissionSuccess(string permissionName)
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        var asset = AssetFixture.CreateAssetDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(
                Arg.Any<AssetWithPermissionsByIdSpec>(),
                Arg.Any<CancellationToken>()
            )
            .Returns(asset);
        var permission = PermissionType.FromName(permissionName);

        // When
        var result = await _handler.Handle(
            new AddOrUpdateAssetPermissionCommand(
                AssetFixture.IdDefault,
                UserFixture.IdDefault,
                UserFixture.IdAlternative,
                permission
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
                    new(AssetFixture.IdDefault, UserFixture.IdAlternative, permission),
                }
            ),
            nameof(asset.Permissions)
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
            new AddOrUpdateAssetPermissionCommand(
                AssetFixture.IdDefault,
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
        PermissionType? adminPermission = adminPermissionName is null
            ? null
            : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);

        // When
        var result = await _handler.Handle(
            new AddOrUpdateAssetPermissionCommand(
                AssetFixture.IdDefault,
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
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((Asset?)null);

        // When
        var result = await _handler.Handle(
            new AddOrUpdateAssetPermissionCommand(
                AssetFixture.IdDefault,
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
