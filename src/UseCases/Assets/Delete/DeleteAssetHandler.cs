using Ardalis.Result;
using Ardalis.SharedKernel;
using MediatR;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.AssetAggregate.Events;
using MiniAssetManagement.Core.AssetAggregate.Specifications;
using MiniAssetManagement.UseCases.Assets.GetPermission;

namespace MiniAssetManagement.UseCases.Assets.Delete;

public class DeleteAssetHandler(
    IRepository<Asset> _repository,
    IMediator _mediator,
    IGetAssetPermissionQueryService _permissionQuery
) : ICommandHandler<DeleteAssetCommand, Result>
{
    public async Task<Result> Handle(
        DeleteAssetCommand request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.AssetId, request.UserId);
        if (permission != PermissionType.Admin && permission != PermissionType.Contributor)
            return Result.Unauthorized();

        var aggregateToDelete = await _repository.FirstOrDefaultAsync(
            new AssetByIdSpec(request.AssetId)
        );
        if (aggregateToDelete is null)
            return Result.NotFound();

        aggregateToDelete.UpdateStatus(AssetStatus.Deleted);

        await _repository.UpdateAsync(aggregateToDelete);
        await _mediator.Publish(new AssetDeletedEvent(request.AssetId));

        return Result.Success();
    }
}
