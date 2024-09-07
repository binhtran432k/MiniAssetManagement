using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Drives.Get;

public record GetDriveQuery(int DriveId, int OwnerId) : IQuery<Result<DriveDTO>>;