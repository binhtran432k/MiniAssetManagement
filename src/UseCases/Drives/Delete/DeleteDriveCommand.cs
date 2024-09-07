using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Drives.Delete;

public record DeleteDriveCommand(int DriveId, int OwnerId) : ICommand<Result>;