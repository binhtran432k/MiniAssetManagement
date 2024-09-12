using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.DriveAggregate;

namespace MiniAssetManagement.UseCases.Assets.Create;

public class CreateFolderFromDriveHandler(
    IRepository<Asset> repository,
    IRepository<Drive> driveRepository
) : ICommandHandler<CreateFolderFromDriveCommand, Result<int>>
{
    public Task<Result<int>> Handle(
        CreateFolderFromDriveCommand request,
        CancellationToken cancellationToken
    )
    {
        return CreateAssetUtils.CreateFromDriveAsync(
            repository,
            driveRepository,
            () => Asset.CreateFolderFromDrive(request.Name, request.DriveId),
            driveId: request.DriveId,
            adminId: request.AdminId,
            cancellationToken
        );
    }
}

public class CreateFileFromDriveHandler(
    IRepository<Asset> repository,
    IRepository<Drive> driveRepository
) : ICommandHandler<CreateFileFromDriveCommand, Result<int>>
{
    public Task<Result<int>> Handle(
        CreateFileFromDriveCommand request,
        CancellationToken cancellationToken
    )
    {
        return CreateAssetUtils.CreateFromDriveAsync(
            repository,
            driveRepository,
            () => Asset.CreateFileFromDrive(request.Name, request.DriveId, request.Type),
            driveId: request.DriveId,
            adminId: request.AdminId,
            cancellationToken
        );
    }
}

public record CreateFolderFromDriveCommand(string Name, int AdminId, int DriveId)
    : ICommand<Result<int>>;

public record CreateFileFromDriveCommand(string Name, int AdminId, int DriveId, FileType Type)
    : ICommand<Result<int>>;
