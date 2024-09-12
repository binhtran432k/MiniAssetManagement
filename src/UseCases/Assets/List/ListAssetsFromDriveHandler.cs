using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;

namespace MiniAssetManagement.UseCases.Assets.List;

public class ListAssetsFromDriveHandler(
    IRepository<Drive> _driveRepository,
    IListAssetsQueryService _query
) : IQueryHandler<ListAssetsFromDriveQuery, Result<(IEnumerable<AssetDTO>, int)>>
{
    public async Task<Result<(IEnumerable<AssetDTO>, int)>> Handle(
        ListAssetsFromDriveQuery request,
        CancellationToken cancellationToken
    )
    {
        bool hasPermission =
            await _driveRepository.CountAsync(
                new DriveByIdAndOwnerIdSpec(request.DriveId, request.UserId)
            ) > 0;
        if (!hasPermission)
            return Result.Unauthorized();

        var result = await _query.ListFromDriveAsync(request.DriveId, request.Skip, request.Take);
        return Result.Success(result);
    }
}
