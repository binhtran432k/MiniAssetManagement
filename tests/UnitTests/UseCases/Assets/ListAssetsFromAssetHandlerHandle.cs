using Ardalis.Result;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Assets;
using MiniAssetManagement.UseCases.Assets.GetPermission;
using MiniAssetManagement.UseCases.Assets.List;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Assets;

public class ListAssetsFromAssetHandlerHandle
{
    private readonly IListAssetsQueryService _service = Substitute.For<IListAssetsQueryService>();
    private readonly IGetAssetPermissionQueryService _permissionService =
        Substitute.For<IGetAssetPermissionQueryService>();
    private ListAssetsFromAssetHandler _handler;

    public ListAssetsFromAssetHandlerHandle() =>
        _handler = new ListAssetsFromAssetHandler(_service, _permissionService);

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task ListsAssetsFromAssetSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        List<AssetDTO> drives = [new(1, "foo", null), new(2, "bar", null)];
        _service
            .ListFromAssetAsync(AssetFixture.ParentIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns((drives, 2));

        // When
        var result = await _handler.Handle(
            new ListAssetsFromAssetQuery(UserFixture.IdDefault, AssetFixture.ParentIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value.Item1, Is.EquivalentTo(drives), nameof(result.Value.Item1));
        Assert.That(result.Value.Item2, Is.EqualTo(2), nameof(result.Value.Item2));
    }

    [Test]
    public async Task ListsAssetsFromAssetFailByUnauthorized()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns((PermissionType?)null);
        List<AssetDTO> drives = [new(1, "foo", null), new(2, "bar", null)];
        _service
            .ListFromAssetAsync(AssetFixture.ParentIdDefault, Arg.Any<int?>(), Arg.Any<int?>())
            .Returns((drives, 2));

        // When
        var result = await _handler.Handle(
            new ListAssetsFromAssetQuery(UserFixture.IdDefault, AssetFixture.ParentIdDefault),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }
}
