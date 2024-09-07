using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Drives.List;

public class ListDrivesHandler(IListDrivesQueryService _query)
    : IQueryHandler<ListDrivesQuery, Result<IEnumerable<DriveDTO>>>
{
    public async Task<Result<IEnumerable<DriveDTO>>> Handle(
        ListDrivesQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _query.ListAsync(request.OwnerId, request.Skip, request.Take);
        return Result.Success(result);
    }
}
