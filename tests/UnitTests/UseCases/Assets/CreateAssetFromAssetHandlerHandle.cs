using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
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
    private readonly CreateFolderFromAssetHandler _folderHandler;
    private readonly CreateFileFromAssetHandler _fileHandler;

    public CreateAssetFromAssetHandlerHandle() =>
        (_folderHandler, _fileHandler) = (
            new CreateFolderFromAssetHandler(_repository, _permissionService),
            new CreateFileFromAssetHandler(_repository, _permissionService)
        );

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    public async Task CreatesFolderFromAssetSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        var folderAsset = AssetFixture.CreateFolderDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(folderAsset);
        var asset = AssetFixture.CreateFolderDefaultFromAsset();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _folderHandler.Handle(
            new CreateFolderFromAssetCommand(
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

    [TestCase(nameof(PermissionType.Admin))]
    [TestCase(nameof(PermissionType.Contributor))]
    public async Task CreatesFileFromAssetSuccess(string adminPermissionName)
    {
        // Given
        _permissionService
            .GetAsync(Arg.Any<int>(), Arg.Any<int>())
            .Returns(PermissionType.FromName(adminPermissionName));
        var folderAsset = AssetFixture.CreateFolderDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(folderAsset);
        var asset = AssetFixture.CreateFileDefaultFromAsset();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _fileHandler.Handle(
            new CreateFileFromAssetCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.ParentIdDefault,
                AssetFixture.FileTypeDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(result.Value, Is.EqualTo(AssetFixture.IdDefault), nameof(result.Value));
    }

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task CreatesFolderFromAssetFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission = adminPermissionName is null
            ? null
            : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);
        var asset = AssetFixture.CreateFolderDefaultFromAsset();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _folderHandler.Handle(
            new CreateFolderFromAssetCommand(
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

    [TestCase(null)]
    [TestCase(nameof(PermissionType.Reader))]
    public async Task CreatesFileFromAssetFailByUnauthorized(string? adminPermissionName)
    {
        // Given
        PermissionType? adminPermission = adminPermissionName is null
            ? null
            : PermissionType.FromName(adminPermissionName);
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(adminPermission);
        var asset = AssetFixture.CreateFileDefaultFromAsset();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _fileHandler.Handle(
            new CreateFileFromAssetCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.ParentIdDefault,
                AssetFixture.FileTypeDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }

    [Test]
    public async Task CreatesFolderFromAssetFailByAddToFile()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        var fileAsset = AssetFixture.CreateFileDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(fileAsset);
        var asset = AssetFixture.CreateFolderDefaultFromAsset();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _folderHandler.Handle(
            new CreateFolderFromAssetCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.ParentIdDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Conflict), nameof(result.Status));
    }

    [Test]
    public async Task CreatesFileFromAssetFailByAddToFile()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        var fileAsset = AssetFixture.CreateFileDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(Arg.Any<AssetByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(fileAsset);
        var asset = AssetFixture.CreateFolderDefaultFromAsset();
        _repository
            .AddAsync(Arg.Any<Asset>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(asset));

        // When
        var result = await _fileHandler.Handle(
            new CreateFileFromAssetCommand(
                AssetFixture.NameDefault,
                UserFixture.IdDefault,
                AssetFixture.ParentIdDefault,
                AssetFixture.FileTypeDefault
            ),
            CancellationToken.None
        );

        // Then
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Conflict), nameof(result.Status));
    }
}
