using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Events;
using MiniAssetManagement.UseCases.Folders.Get;

namespace MiniAssetManagement.UseCases.Folders.Delete;

public class DeleteFolderHandler(
    IRepository<Folder> _repository,
    IMediator _mediator,
    IGetFolderQueryService _query
) : ICommandHandler<DeleteFolderCommand, Result>
{
    public async Task<Result> Handle(
        DeleteFolderCommand request,
        CancellationToken cancellationToken
    )
    {
        var aggregateToDelete = await _query.GetAsync(request.FolderId, request.UserId);
        if (aggregateToDelete == null)
            return Result.NotFound();

        aggregateToDelete.UpdateStatus(FolderStatus.Deleted);

        await _repository.UpdateAsync(aggregateToDelete);
        await _mediator.Publish(new FolderDeletedEvent(request.FolderId));

        return Result.Success();
    }
}
