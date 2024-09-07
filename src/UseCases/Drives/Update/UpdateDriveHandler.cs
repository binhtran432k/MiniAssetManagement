using Ardalis.Result;
using Ardalis.SharedKernel;
using MiniAssetManagement.Core.DriveAggregate;
using MiniAssetManagement.Core.DriveAggregate.Specifications;

namespace MiniAssetManagement.UseCases.Drives.Update;

public class UpdateDriveHandler(IRepository<Drive> _repository)
    : ICommandHandler<UpdateDriveCommand, Result<DriveDTO>>
{
    public async Task<Result<DriveDTO>> Handle(
        UpdateDriveCommand request,
        CancellationToken cancellationToken
    )
    {
        var existingDrive = await _repository.FirstOrDefaultAsync(
            new DriveByIdAndOwnerIdSpec(request.DriveId, request.OwnerId)
        );
        if (existingDrive == null)
            return Result.NotFound();

        existingDrive.UpdateName(request.NewName);
        await _repository.UpdateAsync(existingDrive, cancellationToken);

        return new DriveDTO(existingDrive.Id, existingDrive.Name);
    }
}
