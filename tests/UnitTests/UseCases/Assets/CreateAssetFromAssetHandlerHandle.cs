using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets.Create;
using MiniAssetManagement.UseCases.Assets.GetPermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class CreateAssetFromAssetHandlerHandle
{
    private readonly IRepository<Asset> _repository = Substitute.For<IRepository<Asset>>();
    private readonly IGetAssetPermissionQueryService _permissionService =
        Substitute.For<IGetAssetPermissionQueryService>();
    private readonly CreateAssetFromAssetHandler _handler;

    public CreateAssetFromAssetHandlerHandle() =>
        _handler = new CreateAssetFromAssetHandler(_repository, _permissionService);

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    public async Task CreatesAssetFromAssetSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        var drive = AssetFixture.CreateAssetDefaultFromAsset();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateAssetFromAssetCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.ParentIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(AssetFixture.IdDefault), nameof(result.Value));
    }

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task CreatesAssetFromAssetFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission =
            adminPermissionName is null ? null : PermissionType.FromName(adminPermissionName);
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(adminPermission);
        var drive = AssetFixture.CreateAssetDefaultFromAsset();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(drive));

        // When
        var result = await _handler.Handle(
            new CreateAssetFromAssetCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.ParentIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }
}
