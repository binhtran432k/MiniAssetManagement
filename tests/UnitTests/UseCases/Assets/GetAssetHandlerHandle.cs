using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets.Get;
using MiniAssetManagement.UseCases.Assets.GetPermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class GetAssetHandlerHandle
{
    private readonly IGetAssetPermissionQueryService _permissionService =
        Substitute.For<IGetAssetPermissionQueryService>();
    private readonly IRepository<Asset> _repository = Substitute.For<IRepository<Asset>>();
    private GetAssetHandler _handler;

    public GetAssetHandlerHandle() =>
        _handler = new GetAssetHandler(_repository, _permissionService);

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task GetsAssetSuccess(string adminPermissionName)
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
            new GetAssetQuery(AssetFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.Multiple(() =>
        {
            Assert.That(result.Value.Id, Is.EqualTo(asset.Id), nameof(result.Value.Id));
            Assert.That(result.Value.Name, Is.EqualTo(asset.Name), nameof(result.Value.Name));
        });
    }

    [Test]
    public async Task GetsAssetFailByUnauthorized()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns((PermissionType?)null);

        // When
        var result = await _handler.Handle(
            new GetAssetQuery(AssetFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
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
            new GetAssetQuery(AssetFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}