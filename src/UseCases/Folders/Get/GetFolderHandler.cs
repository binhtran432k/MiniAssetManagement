using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UseCases.Folders.GetPermission;

namespace MiniAssetManagement.UseCases.Folders.Get;

public class GetFolderHandler(
    IRepository<Folder> _repository,
    IGetFolderPermissionQueryService _permissionQuery
) : IQueryHandler<GetFolderQuery, Result<FolderDTO>>
{
    public async Task<Result<FolderDTO>> Handle(
        GetFolderQuery request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.FolderId, request.UserId);
        if (permission is null)
            return Result.Unauthorized();

        var entity = await _repository.FirstOrDefaultAsync(new FolderByIdSpec(request.FolderId));
        if (entity is null)
            return Result.NotFound();

        return new FolderDTO(entity.Id, entity.Name);
    }
}
