using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.UseCases.Folders.GetPermission;

namespace MiniAssetManagement.UseCases.Folders.List;

public class ListFoldersFromFolderHandler(
    IListFoldersQueryService _query,
    IGetFolderPermissionQueryService _permissionQuery
) : IQueryHandler<ListFoldersFromFolderQuery, Result<IEnumerable<FolderDTO>>>
{
    public async Task<Result<IEnumerable<FolderDTO>>> Handle(
        ListFoldersFromFolderQuery request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.FolderId, request.UserId);
        if (permission is null)
            return Result.Unauthorized();

        var result = await _query.ListFromFolderAsync(request.FolderId, request.Skip, request.Take);
        return Result.Success(result);
    }
}