using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Folders.Get;

public class GetFolderHandler(IGetFolderQueryService _query)
    : IQueryHandler<GetFolderQuery, Result<FolderDTO>>
{
    public async Task<Result<FolderDTO>> Handle(
        GetFolderQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _query.GetAsync(request.FolderId, request.UserId);
        if (entity == null)
            return Result.NotFound();

        return new FolderDTO(entity.Id, entity.Name);
    }
}