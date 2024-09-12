using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;

namespace MiniAssetManagement.UseCases.Folders.List;

public class ListFoldersFromDriveHandler(
    IRepository<Drive> _driveRepository,
    IListFoldersQueryService _query
) : IQueryHandler<ListFoldersFromDriveQuery, Result<(IEnumerable<FolderDTO>, int)>>
{
    public async Task<Result<(IEnumerable<FolderDTO>, int)>> Handle(
        ListFoldersFromDriveQuery request,
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
