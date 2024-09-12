using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Events;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets.Delete;
using MiniAssetManagement.UseCases.Assets.GetPermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class DeleteAssetHandlerHandle
{
    private readonly IGetAssetPermissionQueryService _permissionService =
        Substitute.For<IGetAssetPermissionQueryService>();
    private readonly IRepository<Asset> _repository = Substitute.For<IRepository<Asset>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private DeleteAssetHandler _handler;

    public DeleteAssetHandlerHandle() =>
        _handler = new DeleteAssetHandler(_repository, _mediator, _permissionService);

    [TearDown]
    public void TearDown()
    {
        _mediator.ClearReceivedCalls();
    }

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    public async Task DeletesSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        var asset = AssetFixture.CreateAssetDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(asset);

        // When
        var result = await _handler.Handle(
            new DeleteAssetCommand(AssetFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository.Received(1).UpdateAsync(asset, Arg.Any<CancellationToken>());
        await _mediator
            .Received(1)
            .Publish(Arg.Any<AssetDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(asset.Status, Is.EqualTo(AssetStatus.Deleted), nameof(asset.Status));
    }

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task DeletesFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission = adminPermissionName is null
            ? null
            : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);

        // When
        var result = await _handler.Handle(
            new DeleteAssetCommand(AssetFixture.IdInvalid, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>());
        await _mediator
            .DidNotReceive()
            .Publish(Arg.Any<AssetDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }

    [Test]
    public async Task DeletesFailByNotFound()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        _repository
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((Asset?)null);

        // When
        var result = await _handler.Handle(
            new DeleteAssetCommand(AssetFixture.IdInvalid, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>());
        await _mediator
            .DidNotReceive()
            .Publish(Arg.Any<AssetDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}
