using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.Core.FolderAggregate.Specifications;
using MiniAssetManagement.UseCases.Folders.GetPermission;

namespace MiniAssetManagement.UseCases.Folders.Update;

public class UpdateFolderHandler(
    IRepository<Folder> _repository,
    IGetFolderPermissionQueryService _permissionQuery
) : ICommandHandler<UpdateFolderCommand, Result<FolderDTO>>
{
    public async Task<Result<FolderDTO>> Handle(
        UpdateFolderCommand request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.FolderId, request.UserId);
        if (permission != PermissionType.Admin && permission != PermissionType.Contributor)
            return Result.Unauthorized();

        var existingFolder = await _repository.FirstOrDefaultAsync(
            new FolderByIdSpec(request.FolderId)
        );
        if (existingFolder is null)
            return Result.NotFound();

        existingFolder.UpdateName(request.NewName);
        await _repository.UpdateAsync(existingFolder, cancellationToken);

        return new FolderDTO(existingFolder.Id, existingFolder.Name);
    }
}
