using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Drives.List;

public class ListDrivesHandler(IListDrivesQueryService _query)
    : IQueryHandler<ListDrivesQuery, Result<(IEnumerable<DriveDTO>, int)>>
{
    public async Task<Result<(IEnumerable<DriveDTO>, int)>> Handle(
        ListDrivesQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _query.ListAsync(request.OwnerId, request.Skip, request.Take);
        return Result.Success(result);
    }
}
