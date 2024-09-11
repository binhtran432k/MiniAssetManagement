using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Events;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UnitTests.Fixtures;
using MiniAssetManagement.UseCases.Folders.Delete;
using MiniAssetManagement.UseCases.Folders.GetPermission;
using NSubstitute;

namespace MiniAssetManagement.UnitTests.UseCases.Folders;

public class DeleteFolderHandlerHandle
{
    private readonly IGetFolderPermissionQueryService _permissionService =
        Substitute.For<IGetFolderPermissionQueryService>();
    private readonly IRepository<Folder> _repository = Substitute.For<IRepository<Folder>>();
    private readonly IMediator _mediator = Substitute.For<IMediator>();
    private DeleteFolderHandler _handler;

    public DeleteFolderHandlerHandle() =>
        _handler = new DeleteFolderHandler(_repository, _mediator, _permissionService);

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
        var folder = FolderFixture.CreateFolderDefaultFromDrive();
        _repository
            .FirstOrDefaultAsync(Arg.Any<FolderByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns(folder);

        // When
        var result = await _handler.Handle(
            new DeleteFolderCommand(FolderFixture.IdDefault, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository.Received(1).UpdateAsync(folder, Arg.Any<CancellationToken>());
        await _mediator
            .Received(1)
            .Publish(Arg.Any<FolderDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.True, nameof(result.IsSuccess));
        Assert.That(folder.Status, Is.EqualTo(FolderStatus.Deleted), nameof(folder.Status));
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
            new DeleteFolderCommand(FolderFixture.IdInvalid, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>());
        await _mediator
            .DidNotReceive()
            .Publish(Arg.Any<FolderDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.Unauthorized), nameof(result.Status));
    }

    [Test]
    public async Task DeletesFailByNotFound()
    {
        // Given
        _permissionService.GetAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(PermissionType.Admin);
        _repository
            .FirstOrDefaultAsync(Arg.Any<FolderByIdSpec>(), Arg.Any<CancellationToken>())
            .Returns((Folder?)null);

        // When
        var result = await _handler.Handle(
            new DeleteFolderCommand(FolderFixture.IdInvalid, UserFixture.IdDefault),
            CancellationToken.None
        );

        // Then
        await _repository
            .DidNotReceive()
            .UpdateAsync(Arg.Any<Folder>(), Arg.Any<CancellationToken>());
        await _mediator
            .DidNotReceive()
            .Publish(Arg.Any<FolderDeletedEvent>(), Arg.Any<CancellationToken>());
        Assert.That(result.IsSuccess, Is.False, nameof(result.IsSuccess));
        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound), nameof(result.Status));
    }
}
