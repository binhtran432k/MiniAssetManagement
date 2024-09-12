using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets.GetPermission;
using MiniAssetManagement.UseCases.Assets.Update;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class UpdateAssetHandlerHandle
{
    private readonly IGetAssetPermissionQueryService _permissionService =
        Substitute.For<IGetAssetPermissionQueryService>();
    private readonly IRepository<Asset> _repository = Substitute.For<IRepository<Asset>>();
    private UpdateAssetHandler _handler;

    public UpdateAssetHandlerHandle() =>
        _handler = new UpdateAssetHandler(_repository, _permissionService);

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    public async Task UpdatesAssetSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        var asset = AssetFixture.CreateFolderDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(asset);

        // When
        var result = await _handler.Handle(
            new UpdateAssetCommand(
                AssetFixture.IdDefault,
                UserFixture.IdDefault,
                AssetFixture.NameNew
            ),
            CancellationToken.None
        );

        // Then
        await _repository.Received(1).UpdateAsync(asset, Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.Multiple(() =>
        {
            Assert.That(
                result.Value.Id,
                Is.EqualTo(AssetFixture.IdDefault),
                nameof(result.Value.Id)
            );
            Assert.That(
                result.Value.Name,
                Is.EqualTo(AssetFixture.NameNew),
                nameof(result.Value.Name)
            );
        });
    }

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task UpdatesAssetFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission =
            adminPermissionName is null ? null : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);

        // When
        var result = await _handler.Handle(
            new UpdateAssetCommand(
                AssetFixture.IdInvalid,
                UserFixture.IdDefault,
                AssetFixture.NameDefault
            ),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }

    [Test]
    public async Task GetsAssetFailByNotFound()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        _repository
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((Asset?)null);

        // When
        var result = await _handler.Handle(
            new UpdateAssetCommand(
                AssetFixture.IdInvalid,
                UserFixture.IdDefault,
                AssetFixture.NameDefault
            ),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}