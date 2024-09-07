using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Drives.Update;

public record UpdateDriveCommand(int DriveId, int OwnerId, string NewName) : ICommand<Result<DriveDTO>>;