using Ardalis.Result;
using Ardalis.SharedKernel;

namespace MiniAssetManagement.UseCases.Drives.List;

public record ListDrivesQuery(int OwnerId, int? Skip = null, int? Take = null)
    : IQuery<Result<IEnumerable<DriveDTO>>>;
