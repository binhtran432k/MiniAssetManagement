using Ardalis.SharedKernel;
using MediatR;
using Microsoft.Extensions.Logging;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Events;
using MiniAssetManagement.Core.AssetAggregate.Handlers;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.Core.Handlers;

public class AssetDeletedNotificationHandlerHandle
{
    private readonly ILogger<AssetDeletedEvent> _logger = Substitute.For<
        ILogger<AssetDeletedEvent>
    >();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private readonly IRepository<Asset> _assetRepository = Substitute.For<IRepository<Asset>>();
    private AssetDeletedNotificationHandler _handler;

    public AssetDeletedNotificationHandlerHandle() =>
        _handler = new(_logger, _mediator, _assetRepository);

    [Test]
    public async Task DeleteAssetsMatchOwnerId()
    {
        // Given
        var asset1 = Asset.CreateFolderFromAsset("a", 1);
        var asset2 = Asset.CreateFolderFromAsset("b", 1);
        List<Asset> assets = new() { asset1, asset2 };
        _assetRepository
            .ListAsync(Arg.Any<AssetsByParentIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(assets);

        // When
        await _handler.Handle(new(UserFixture.IdToDelete), CancellationToken.None);

        // Then
        await _assetRepository
            .Received(2)
            .UpdateAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>());
        await _mediator
            .Received(2)
            .Publish(Arg.Any<AssetDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.Multiple(() =>
        {
            Assert.That(asset1.Status, Is.EqualTo(AssetStatus.Deleted), nameof(asset1.Status));
            Assert.That(asset2.Status, Is.EqualTo(AssetStatus.Deleted), nameof(asset2.Status));
        });
    }
}