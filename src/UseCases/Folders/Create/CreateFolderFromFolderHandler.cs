using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;
using MiniAssetManagement.UseCases.Folders.GetPermission;

namespace MiniAssetManagement.UseCases.Folders.Create;

public class CreateFolderFromFolderHandler(
    IRepository<Folder> _repository,
    IGetFolderPermissionQueryService _permissionQuery
) : ICommandHandler<CreateFolderFromFolderCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateFolderFromFolderCommand request,
        CancellationToken cancellationToken
    )
    {
        var permission = await _permissionQuery.GetAsync(request.FolderId, request.AdminId);
        if (permission != PermissionType.Admin && permission != PermissionType.Contributor)
            return Result.Unauthorized();

        var newFolder = Folder.CreateFromFolder(request.Name, request.FolderId);
        newFolder.AddOrUpdatePermission(request.AdminId, PermissionType.Admin);
        var createdItem = await _repository.AddAsync(newFolder, cancellationToken);

        return createdItem.Id;
    }
}