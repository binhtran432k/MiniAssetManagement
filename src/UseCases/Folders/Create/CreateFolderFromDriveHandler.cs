using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;
using MiniAssetManagement.Core.FolderAggregate;

namespace MiniAssetManagement.UseCases.Folders.Create;

public class CreateFolderFromDriveHandler(
    IRepository<Folder> _repository,
    IRepository<Drive> _driveRepository
) : ICommandHandler<CreateFolderFromDriveCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateFolderFromDriveCommand request,
        CancellationToken cancellationToken
    )
    {
        bool hasPermission =
            await _driveRepository.CountAsync(
                new DriveByIdAndOwnerIdSpec(request.DriveId, request.AdminId)
            ) > 0;
        if (!hasPermission)
            return Result.Unauthorized();

        var newFolder = Folder.CreateFromDrive(request.Name, request.DriveId);
        newFolder.AddOrUpdatePermission(request.AdminId, PermissionType.Admin);
        var createdItem = await _repository.AddAsync(newFolder, cancellationToken);

        return createdItem.Id;
    }
}
