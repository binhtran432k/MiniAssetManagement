using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.List;

public class ListFoldersFromFolderHandler(IListFoldersQueryService _query)
    : IQueryHandler<ListFoldersFromFolderQuery, Result<IEnumerable<FolderDTO>>>
{
    public async Task<Result<IEnumerable<FolderDTO>>> Handle(
        ListFoldersFromFolderQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await _query.ListFromFolderAsync(
            request.UserId,
            request.FolderId,
            request.Skip,
            request.Take
        );
        return Result.Success(result);
    }
}