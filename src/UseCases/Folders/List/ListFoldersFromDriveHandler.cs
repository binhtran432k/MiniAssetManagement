using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.List;

public class ListFoldersFromDriveHandler(IListFoldersQueryService _query)
    : IQueryHandler<ListFoldersFromDriveQuery, Result<IEnumerable<FolderDTO>>>
{
    public async Task<Result<IEnumerable<FolderDTO>>> Handle(
        ListFoldersFromDriveQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _query.ListFromDriveAsync(
            request.UserId,
            request.DriveId,
            request.Skip,
            request.Take
        );
        return Result.Success(result);
    }
}