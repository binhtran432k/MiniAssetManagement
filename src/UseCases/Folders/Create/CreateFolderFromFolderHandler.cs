using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.UseCases.Folders.Create;

public class CreateFolderFromFolderHandler(IRepository<Folder> _repository)
    : ICommandHandler<CreateFolderFromFolderCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateFolderFromFolderCommand request,
        CancellationToken cancellationToken
    )
    {
        var newFolder = Folder.CreateFromFolder(request.Name, request.FolderId);
        newFolder.AddOrUpdatePermission(request.AdminId, PermissionType.Admin);
        var createdItem = await _repository.AddAsync(newFolder, cancellationToken);

        return createdItem.Id;
    }
}