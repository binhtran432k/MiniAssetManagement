using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.AssetAggregate;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;

namespace MiniAssetManagement.UseCases.Assets.Create;

public class CreateAssetFromDriveHandler(
    IRepository<Asset> _repository,
    IRepository<Drive> _driveRepository
) : ICommandHandler<CreateAssetFromDriveCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        CreateAssetFromDriveCommand request,
        CancellationToken cancellationToken
    )
    {
        bool hasPermission =
            await _driveRepository.CountAsync(
                new DriveByIdAndOwnerIdSpec(request.DriveId, request.AdminId)
            ) > 0;
        if (!hasPermission)
            return Result.Unauthorized();

        var newAsset = Asset.CreateFromDrive(request.Name, request.DriveId);
        newAsset.AddOrUpdatePermission(request.AdminId, PermissionType.Admin);
        var createdItem = await _repository.AddAsync(newAsset, cancellationToken);

        return createdItem.Id;
    }
}
