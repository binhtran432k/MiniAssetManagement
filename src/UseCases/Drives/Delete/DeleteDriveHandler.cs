using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Events;
using MiniAssetManagement.Core.DriveAggregate.Specifications;

namespace MiniAssetManagement.UseCases.Drives.Delete;

public class DeleteDriveHandler(IRepository<Drive> _repository, IMediator _mediator)
    : ICommandHandler<DeleteDriveCommand, Result>
{
    public async Task<Result> Handle(
        DeleteDriveCommand request,
        CancellationToken cancellationToken
    )
    {
        var aggregateToDelete = await _repository.FirstOrDefaultAsync(
            new DriveByIdAndOwnerIdSpec(request.DriveId, request.OwnerId)
        );
        if (aggregateToDelete == null)
            return Result.NotFound();

        aggregateToDelete.UpdateStatus(DriveStatus.Deleted);

        await _repository.UpdateAsync(aggregateToDelete);
        await _mediator.Publish(new DriveDeletedEvent(request.DriveId));

        return Result.Success();
    }
}
