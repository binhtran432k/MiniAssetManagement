using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Events;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UseCases.Folders.GetPermission;

namespace MiniAssetManagement.UseCases.Folders.Delete;

public class DeleteFolderHandler(
    IRepository<Folder> _repository,
    IMediator _mediator,
    IGetFolderPermissionQueryService _permissionQuery
) : ICommandHandler<DeleteFolderCommand, Result>
{
    public async Task<Result> Handle(
        DeleteFolderCommand request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.FolderId, request.UserId);
        if (permission != PermissionType.Admin && permission != PermissionType.Contributor)
            return Result.Unauthorized();

        var aggregateToDelete = await _repository.FirstOrDefaultAsync(
            new FolderByIdSpec(request.FolderId)
        );
        if (aggregateToDelete is null)
            return Result.NotFound();

        aggregateToDelete.UpdateStatus(FolderStatus.Deleted);

        await _repository.UpdateAsync(aggregateToDelete);
        await _mediator.Publish(new FolderDeletedEvent(request.FolderId));

        return Result.Success();
    }
}