using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.UseCases.Folders.Create;

public class CreateFolderFromDriveHandler(IRepository<Folder> _repository)
    : ICommandHandler<CreateFolderFromDriveCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateFolderFromDriveCommand request,
        CancellationToken cancellationToken
    )
    {
        var newFolder = Folder.CreateFromDrive(request.Name, request.DriveId);
        newFolder.AddPermission(new(request.AdminId, PermissionType.Admin));
        var createdItem = await _repository.AddAsync(newFolder, cancellationToken);

        return createdItem.Id;
    }
}
