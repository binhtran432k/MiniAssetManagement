using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;

namespace MiniAssetManagement.UseCases.Drives.Get;

public class GetDriveHandler(IReadRepository<Drive> _repository)
    : IQueryHandler<GetDriveQuery, Result<DriveDTO>>
{
    public async Task<Result<DriveDTO>> Handle(
        GetDriveQuery request,
        CancellationToken cancellationToken
    )
    {
        DriveByIdAndOwnerIdSpec spec = new(request.DriveId, request.OwnerId);
        var entity = await _repository.FirstOrDefaultAsync(spec, cancellationToken);
        if (entity is null)
            return Result.NotFound();

        return new DriveDTO(entity.Id, entity.Name);
    }
}
